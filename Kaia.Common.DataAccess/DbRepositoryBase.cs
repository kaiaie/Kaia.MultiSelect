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
        private readonly QueryHelper _queryHelper;

        private DbRepositoryBase()
        {
            _logger = LogManager.GetLogger(GetType().FullName);
            _queryHelper = QueryHelper.Default;
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
        protected QueryHelper QueryHelper { get { return _queryHelper; } }

        public virtual long Create(TNewEntity newEntity)
        {
            var query = QueryHelper.GetInsertQuery<TEntity>();
            if (Logger.IsTraceEnabled) Logger.Trace(query);
            using (var reader = Connection.ExecuteReader(query.Sql, 
                newEntity, Transaction))
            {
                reader.Read();
                return (long)reader[0];
            }
        }


        public virtual IEnumerable<TEntity> Get(IEnumerable<long> ids)
        {
            var query = QueryHelper.GetSelectManyQuery<TEntity>(ids);
            if (Logger.IsTraceEnabled) Logger.Trace(query.Sql);
            using (var reader = Connection.ExecuteReader(query.Sql, 
                query.Parameters, Transaction))
            {
                while (reader.Read())
                {
                    yield return EntityBuilder.Build<TEntity>(reader);
                }
            }
        }


        public virtual TEntity Get(long id)
        {
            var sql = string.Concat(QueryHelper.GetSelectAllQuery<TEntity>().Sql,
                 " WHERE ", QueryHelper.GetKeyColumnName<TEntity>(), " = @id");
            if (Logger.IsTraceEnabled) Logger.Trace(sql);
            using (var reader = Connection.ExecuteReader(sql, new { id }, 
                Transaction))
            {
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
        }


        public virtual IEnumerable<TEntity> GetAll()
        {
            var query = QueryHelper.GetSelectAllQuery<TEntity>();
            if (Logger.IsTraceEnabled) Logger.Trace(query);
            using (var reader = Connection.ExecuteReader(query.Sql, null, 
                Transaction))
            {
                while (reader.Read())
                {
                    yield return EntityBuilder.Build<TEntity>(reader);
                }
            }
        }


        public virtual long Modify(TEntityModifier entitiesToUpdate)
        {
            QueryComponents query = null;
            switch (entitiesToUpdate.ModificationType)
            {
                case ModificationType.Update:
                    query = 
                        QueryHelper.GetUpdateQuery(entitiesToUpdate);
                    break;
                case ModificationType.Duplicate:
                    query =
                        QueryHelper.GetDuplicateQuery(entitiesToUpdate);
                    break;
                case ModificationType.Delete:
                    query =
                        QueryHelper.GetDeleteQuery(entitiesToUpdate);
                    break;
            }
            return Connection.Execute(query.Sql, query.Parameters, Transaction);
        }
    }
}
