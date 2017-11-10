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
            throw new NotImplementedException();
        }

        public virtual TEntity Get(long id)
        {
            var sql = string.Concat(QueryHelper.Default.GetSelectStatement<TEntity>(),
                 " WHERE ", QueryHelper.Default.GetKeyColumnName<TEntity>(), " = @id");
            Logger.Trace(sql);
            using (var reader = Connection.ExecuteReader(sql,
                new { id },
                Transaction))
                if (reader.Read())
                {
                    // Because entity objects are immutable, we need to call 
                    // the constructor, but arguments to the constructor and 
                    // fields returned from the database may not be
                    // in the same order
                    // TODO Factor this out into a builder library
                    var argValues = new List<Tuple<int, object>>();
                    var constructor = typeof(TEntity).GetConstructors().First();
                    var constructorArgs = constructor.GetParameters();
                    for (var i = 0; i < reader.FieldCount; ++i)
                    {
                        var fieldName = reader.GetName(i).ToCamelCase();
                        var arg = constructorArgs.Single(a => a.Name == fieldName);
                        argValues.Add(new Tuple<int, object>(arg.Position, reader[i]));
                    }
                    var @params = argValues
                        .OrderBy(v => v.Item1).Select(v => v.Item2).ToArray();
                    return constructor.Invoke(@params) as TEntity;
                }
                else
                {
                    throw new Exception("Not found");
                }
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            var sql = QueryHelper.Default.GetSelectStatement<TEntity>();
            Logger.Trace(sql);
            using (var reader = Connection.ExecuteReader(sql,
                transaction: Transaction))
                while (reader.Read())
                {
                    // Because entity objects are immutable, we need to call 
                    // the constructor, but arguments to the constructor and 
                    // fields returned from the database may not be
                    // in the same order
                    // TODO Factor this out into a builder library
                    var argValues = new List<Tuple<int, object>>();
                    var constructor = typeof(TEntity).GetConstructors().First();
                    var constructorArgs = constructor.GetParameters();
                    for (var i = 0; i < reader.FieldCount; ++i)
                    {
                        var fieldName = reader.GetName(i).ToCamelCase();
                        var arg = constructorArgs.Single(a => a.Name == fieldName);
                        argValues.Add(new Tuple<int, object>(arg.Position, reader[i]));
                    }
                    var @params = argValues
                        .OrderBy(v => v.Item1).Select(v => v.Item2).ToArray();
                    yield return constructor.Invoke(@params) as TEntity;
                }
        }

        public virtual void Modify(TEntityModifier entitiesToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
