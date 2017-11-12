using Dapper;
using Kaia.Common.DataAccess.Contract;
using Kaia.Common.DataAccess.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kaia.Common.DataAccess
{
    /// <summary>
    /// Implements various helper methods for building SQL queries
    /// </summary>
    public class QueryHelper
    {
        private static QueryHelper _queryHelper;

        // TODO Create method to allow per-RDBMS versions of the 
        // QueryHelper to account for SQL dialect difference
        public static QueryHelper Default
        {
            get
            {
                if (_queryHelper == null)
                {
                    _queryHelper = new QueryHelper();
                }
                return _queryHelper;
            }
        }


        private IEnumerable<PropertyInfo> GetEntityProperties(Type t)
        {
            return t.GetProperties(BindingFlags.Public |
                BindingFlags.Instance);
        }


        private IEnumerable<PropertyInfo>GetEntityProperties<T>()
        {
            return GetEntityProperties(typeof(T));
        }


        public IEnumerable<string> GetPropertyNames<T>()
        {
            return GetEntityProperties<T>()
                .Where(p => !p.GetCustomAttributes()
                    .Any(a => a.GetType() == typeof(IgnoreAttribute)))
                .Select(p => p.Name);
        }


        public IEnumerable<string> GetAllColumnNames<T>()
        {
            return GetPropertyNames<T>().Select(p => p.ToSnakeCaseLower());
        }


        public IEnumerable<string> GetNonKeyColumnNames<T>()
        {
            var keyPropertyName = GetKeyPropertyName<T>();
            return GetPropertyNames<T>()
                .Where(p => p != keyPropertyName)
                .Select(p => p.ToSnakeCaseLower());
        }


        public string GetTableName<T>()
        {
            var type = typeof(T);
            var typeName = type.Name;
            if (type.GetCustomAttributes()
                .Any(a => a.GetType() == typeof(TableNameAttribute)))
            {
                return type.GetCustomAttributes<TableNameAttribute>()
                    .First().TableName;
            }
            if (typeof(IEntityModifier).IsAssignableFrom(type) && 
                typeName.EndsWith("Modifier", StringComparison.InvariantCultureIgnoreCase))
            {
                typeName = typeName.Substring(0, typeName.Length - 8);
            }
            return typeName.ToPlural().ToSnakeCaseLower();
        }


        public string GetKeyPropertyName<T>()
        {
            var entityType = typeof(T);
            var entityTypeAttribute = entityType.GetCustomAttributes()
                .FirstOrDefault(a => a.GetType() == typeof(EntityAttribute))
                as EntityAttribute;
            if (entityTypeAttribute != null)
            {
                entityType = entityTypeAttribute.EntityType;
            }

            return GetEntityProperties(entityType)
                .First(p => p.GetCustomAttributes() // XXX Assume only one key (for now!)
                    .Any(a => a.GetType() == typeof(KeyAttribute)))
                .Name;
        }


        public string GetKeyColumnName<T>()
        {
            return GetKeyPropertyName<T>().ToSnakeCaseLower();
        }


        public IEnumerable<PropertyInfo> GetUpdatableProperties<T>()
            where T : IEntityModifier
        {
            return GetEntityProperties<T>()
                .Where(pi => pi.PropertyType.IsGenericType &&
                    pi.PropertyType.GetGenericTypeDefinition() == typeof(UpdatableField<>));
        }


        public QueryComponents GetInsertQuery<T>()
        {
            var sqlb = new StringBuilder("INSERT INTO ");
            sqlb.Append(GetTableName<T>()).AppendLine("(");
            var nonKeyColumns = GetNonKeyColumnNames<T>().ToList();
            var isFirst = true;
            foreach (var column in nonKeyColumns)
            {
                sqlb.AppendLine();
                if (!isFirst) sqlb.Append(", ");
                sqlb.Append(column);
                isFirst = false;
            }
            sqlb.AppendLine(")").AppendLine("VALUES").AppendLine("(");
            isFirst = true;
            foreach (var column in nonKeyColumns)
            {
                sqlb.AppendLine();
                if (!isFirst) sqlb.Append(", ");
                sqlb.Append("@").Append(column.ToPascalCase());
                isFirst = false;
            }
            sqlb.AppendLine(");")
                .AppendLine("SELECT last_insert_rowid()");
            return new QueryComponents(sqlb.ToString());
        }


        public QueryComponents GetSelectAllQuery<T>()
        {
            var result = new StringBuilder();
            result.Append("SELECT ").Append(string.Join(", ", GetAllColumnNames<T>()))
                .AppendLine(" FROM ").Append(GetTableName<T>());

            return new QueryComponents(result.ToString());
        }


        public QueryComponents GetSelectManyQuery<T>(IEnumerable<long> ids)
        {
            var @params = new DynamicParameters();
            var sql = string.Concat(GetSelectAllQuery<T>().Sql,
                 " WHERE ", GetIdsWhereClause<T>(ids, @params).Sql);
            return new QueryComponents(sql, @params);
        }


        public QueryComponents GetIdsWhereClause<T>(IEnumerable<long> ids, 
            DynamicParameters @params = null)
        {
            // According to Dapper doc, it should be possible to pass an 
            // IEnumerable as a parameter to one of the Query methods and have 
            // it do the Right Thing but it throws an error with SQLite which 
            // doesn't seem to like parameterised IN expressions... do it the 
            // long way :-(
            if (@params == null) @params = new DynamicParameters();
            var paramNames = new List<string>();
            var paramNo = 1;
            foreach (var id in ids)
            {
                var paramName = string.Format("p{0}", paramNo);
                @params.Add(paramName, id);
                paramNames.Add(string.Concat("@", paramName));
                paramNo++;
            }
            var keyColumnName = GetKeyColumnName<T>();
            var sql = string.Join(" OR ",
                paramNames.Select(s => string.Format("{0} = {1}", keyColumnName, s)));
            return new QueryComponents(sql, @params);
        }


        public QueryComponents GetDeleteQuery<T>(T entitiesToUpdate)
            where T : IEntityModifier
        {
            var @params = new DynamicParameters();
            var sql = string.Concat("DELETE FROM ", GetTableName<T>(), 
                "WHERE ", GetIdsWhereClause<T>(entitiesToUpdate.Ids, @params).Sql);
            return new QueryComponents(sql, @params);
        }


        public QueryComponents GetDuplicateQuery<T>(T entitiesToUpdate)
            where T : IEntityModifier
        {
            var tableName = GetTableName<T>();
            var @params = new DynamicParameters();
            var props = GetUpdatableProperties<T>();
            var insertColsSql = string.Concat("INSERT INTO ", tableName, 
                Environment.NewLine, "(", 
                string.Join(", ", props.Select(p => p.Name.ToSnakeCaseLower())), 
                ")");
            var selectSql = new StringBuilder(" SELECT ");
            var isFirst = true;
            foreach (var prop in props)
            {
                if (!isFirst) selectSql.Append(", ");
                if (IsPropertyUpdated(prop, entitiesToUpdate))
                {
                    selectSql.AppendFormat("@{0} AS {1}", prop.Name, 
                        prop.Name.ToSnakeCaseLower());
                    @params.Add(prop.Name, GetUpdatablePropertyValue(prop, 
                        entitiesToUpdate));
                }
                else
                {
                    selectSql.Append(prop.Name.ToSnakeCaseLower());
                }
                isFirst = false;
            }
            selectSql.Append(" FROM ").Append(tableName);
            var whereClause = GetIdsWhereClause<T>(entitiesToUpdate.Ids, @params);
            return new QueryComponents(
                string.Concat(insertColsSql, selectSql.ToString(), " WHERE ", 
                    whereClause.Sql), 
                @params);
        }


        private object GetUpdatablePropertyValue(PropertyInfo pi, object obj)
        {
            var propValue = pi.GetValue(obj);
            var propType = propValue.GetType();
            var propProp = propType.GetProperty("Value",
                BindingFlags.Public | BindingFlags.Instance);
            if (propProp != null)
            {
                return propProp.GetValue(propValue);
            }
            return null;
        }


        private bool IsPropertyUpdated(PropertyInfo pi, object obj)
        {
            var result = false;
            var propVal = pi.GetValue(obj) as IUpdatable;
            if (propVal != null)
            {
                return propVal.IsUpdated;
            }
            return result;
        }


        public QueryComponents GetUpdateQuery<T>(T entitiesToUpdate)
            where T : IEntityModifier
        {
            var tableName = GetTableName<T>();
            var @params = new DynamicParameters();
            var props = GetUpdatableProperties<T>();
            var updateSql = string.Concat("UPDATE ", tableName);
            var setSql = new StringBuilder(" SET ");
            var isFirst = true;
            foreach (var prop in props)
            {
                if (!isFirst) setSql.Append(", ");
                if (IsPropertyUpdated(prop, entitiesToUpdate))
                {
                    setSql.AppendFormat("{1} = @{0}", prop.Name,
                        prop.Name.ToSnakeCaseLower());
                    @params.Add(prop.Name, GetUpdatablePropertyValue(prop,
                        entitiesToUpdate));
                    isFirst = false;
                }
            }
            var whereClause = GetIdsWhereClause<T>(entitiesToUpdate.Ids, @params);
            return new QueryComponents(
                string.Concat(updateSql, setSql.ToString(), " WHERE ",
                    whereClause.Sql),
                @params);
        }
    }
}
