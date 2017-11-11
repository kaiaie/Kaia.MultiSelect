using Dapper;
using Kaia.Common.DataAccess.Contract;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Kaia.Common.DataAccess
{
    public abstract class DbRepositoryBase<TEntity, TNewEntity, TEntityModifier> :
        IRepository<TEntity, TNewEntity, TEntityModifier> 
        where TEntity: class
        where TEntityModifier : IEntityModifier
    {
        private readonly ILogger _logger;
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        private DbRepositoryBase()
        {
            _logger = LogManager.GetLogger(GetType().FullName);
        }

        public DbRepositoryBase(IDbConnection connection, IDbTransaction transaction) : 
            this()
        {
            _connection = connection;
            _transaction = transaction;
        }

        protected ILogger Logger { get { return _logger; } }
        protected IDbConnection Connection { get { return _connection; } }
        protected IDbTransaction Transaction { get { return _transaction; } }

        public virtual void Create(TNewEntity newEntity)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<TEntity> Get(IEnumerable<long> ids)
        {
            // According to Dapper doc, it should be possible to pass an 
            // IEnumerable as a Query and have it Just Work but it throws an 
            // error with SQLite which doesn't seem to like parameterised 
            // IN expressions... do it the long way
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
            var keyColumnName = QueryHelper.Default.GetKeyColumnName<TEntity>();
            var sql = string.Concat(QueryHelper.Default.GetSelectStatement<TEntity>(),
                 " WHERE ", string.Join(" OR ", 
                    paramNames.Select(s => string.Format("{0} = {1}", keyColumnName, s))));
            if (Logger.IsTraceEnabled) Logger.Trace(sql);
            using (var reader = Connection.ExecuteReader(sql,
                @params,
                Transaction))
                while (reader.Read())
                {
                    yield return EntityBuilder.Build<TEntity>(reader);
                }
        }

        public virtual TEntity Get(long id)
        {
            var sql = string.Concat(QueryHelper.Default.GetSelectStatement<TEntity>(),
                 " WHERE ", QueryHelper.Default.GetKeyColumnName<TEntity>(), " = @id");
            if (Logger.IsTraceEnabled) Logger.Trace(sql);
            using (var reader = Connection.ExecuteReader(sql,
                new { id },
                Transaction))
                if (reader.Read())
                {
                    return EntityBuilder.Build<TEntity>(reader);
                }
                else
                {
                    var exc = new EntityNotFoundException();
                    exc.Data["Kaia.Entity"] = typeof(TEntity).FullName;
                    exc.Data["Kaia.Id"] = id;
                    throw exc;
                }
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            var sql = QueryHelper.Default.GetSelectStatement<TEntity>();
            if (Logger.IsTraceEnabled) Logger.Trace(sql);
            using (var reader = Connection.ExecuteReader(sql,
                transaction: Transaction))
                while (reader.Read())
                {
                    yield return EntityBuilder.Build<TEntity>(reader);
                }
        }

        public virtual void Modify(TEntityModifier entitiesToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
