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


        private IEnumerable<PropertyInfo>GetEntityProperties<T>()
        {
            return typeof(T).GetProperties(BindingFlags.Public | 
                BindingFlags.Instance);
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
            return GetEntityProperties<T>()
                .First(p => p.GetCustomAttributes() // XXX Assume only one key (for now!)
                    .Any(a => a.GetType() == typeof(KeyAttribute)))
                .Name;
        }


        public string GetKeyColumnName<T>()
        {
            return GetKeyPropertyName<T>().ToSnakeCaseLower();
        }


        public IEnumerable<PropertyInfo> GetUpdatedProperties<T>(T entityModifier)
            where T : IEntityModifier
        {
            return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(pi => pi.PropertyType.IsGenericType &&
                    pi.PropertyType.GetGenericTypeDefinition() == typeof(UpdatableField<>) && 
                    ((IUpdatable)pi.GetValue(entityModifier)).IsUpdated);
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
            // According to Dapper doc, it should be possible to pass an 
            // IEnumerable as a parameter to one of the Query methods and have 
            // it do the Right Thing but it throws an error with SQLite which 
            // doesn't seem to like parameterised IN expressions... do it the 
            // long way :-(
            var @params = new DynamicParameters();
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
            var sql = string.Concat(GetSelectAllQuery<T>().Sql,
                 " WHERE ", string.Join(" OR ",
                    paramNames.Select(s => string.Format("{0} = {1}", keyColumnName, s))));
            return new QueryComponents(sql, @params);
        }

        public QueryComponents GetDeleteQuery<T>(T entitiesToUpdate)
            where T : IEntityModifier
        {
            throw new NotImplementedException();
        }

        public QueryComponents GetDuplicateQuery<T>(T entitiesToUpdate)
            where T : IEntityModifier
        {
            throw new NotImplementedException();
        }

        public QueryComponents GetUpdateQuery<T>(T entitiesToUpdate)
            where T : IEntityModifier
        {
            throw new NotImplementedException();
        }
    }
}
