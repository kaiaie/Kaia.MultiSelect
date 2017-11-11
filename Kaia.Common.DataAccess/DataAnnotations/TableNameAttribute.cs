using System;

namespace Kaia.Common.DataAccess.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TableNameAttribute : Attribute
    {
        private readonly string _tableName;

        public TableNameAttribute(string tableName)
        {
            _tableName = tableName;
        }

        public string TableName { get { return _tableName; } }
    }
}
