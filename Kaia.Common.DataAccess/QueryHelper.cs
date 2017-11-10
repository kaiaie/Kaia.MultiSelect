using Kaia.Common.DataAccess.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Common.DataAccess
{
    public class QueryHelper
    {
        private static QueryHelper _queryHelper;

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


        public IEnumerable<string> GetColumnNames<T>()
        {
            return GetPropertyNames<T>().Select(p => p.ToSnakeCaseLower());
        }


        public string GetTableName<T>()
        {
            return typeof(T).Name.ToPlural().ToSnakeCaseLower();
        }


        public string GetSelectStatement<T>()
        {
            var result = new StringBuilder();
            result.Append("SELECT ").Append(string.Join(", ", GetColumnNames<T>()))
                .AppendLine(" FROM ").Append(GetTableName<T>());

            return result.ToString();
        }

        public string GetKeyPropertyName<T>()
        {
            return GetEntityProperties<T>()
                .First(p => p.GetCustomAttributes()
                    .Any(a => a.GetType() == typeof(KeyAttribute)))
                .Name;
        }

        public string GetKeyColumnName<T>()
        {
            return GetKeyPropertyName<T>().ToSnakeCaseLower();
        }
    }
}
