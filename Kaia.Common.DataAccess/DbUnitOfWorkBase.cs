using Kaia.Common.DataAccess.Contract;
using NLog;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace Kaia.Common.DataAccess
{
    public abstract class DbUnitOfWorkBase : IUnitOfWork
    {
        private readonly ILogger _logger;
        protected readonly IDbConnection _connection;
        protected IDbTransaction _transaction;

        private DbUnitOfWorkBase()
        {
            _logger = LogManager.GetLogger(GetType().FullName);
        }

        public DbUnitOfWorkBase(string providerName, string connectionString) : this()
        {
            var dbf = DbProviderFactories.GetFactory(providerName);
            _connection = dbf.CreateConnection();
            _connection.ConnectionString = connectionString;
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public DbUnitOfWorkBase(ConnectionStringSettings connectionString) :
            this(connectionString.ProviderName, connectionString.ConnectionString)
        { }


        protected ILogger Logger { get { return _logger; } }
        protected IDbConnection Connection { get { return _connection; } }
        protected IDbTransaction Transaction { get { return _transaction; } }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction.Dispose();
            }
            if (_connection != null)
            {
                if (_connection.State != ConnectionState.Closed)
                {
                    _connection.Close();
                }
                _connection.Dispose();
            }
        }

        public virtual void Save()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
                _transaction.Dispose();
            }
            if (_connection != null)
            {
                _transaction = _connection.BeginTransaction();
            }
        }
    }
}
