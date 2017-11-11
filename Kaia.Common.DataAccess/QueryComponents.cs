using Dapper;

namespace Kaia.Common.DataAccess
{
    /// <summary>
    /// A simple class used by the QueryHelper class to pass SQL query text
    /// and optional associated parameters
    /// </summary>
    public sealed class QueryComponents
    {
        private readonly string _sql;
        private readonly DynamicParameters _parameters;

        public string Sql { get { return _sql; } }
        public DynamicParameters Parameters { get { return _parameters; } }

        
        public QueryComponents(string sql, DynamicParameters parameters = null)
        {
            _sql = sql;
            _parameters = parameters;
        }
    }
}
