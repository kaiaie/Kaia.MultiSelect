using System.Collections.Generic;

namespace Kaia.Common.DataAccess.Contract
{
    public interface IRepository<TEntity, TNewEntity, TEntityModifier>
        where TEntity : class
        where TEntityModifier : IEntityModifier
    {
        IEnumerable<TEntity> GetAll();


        TEntity Get(long id);

        IEnumerable<TEntity> Get(IEnumerable<long> ids);

        long Create(TNewEntity newEntity);

        long Modify(TEntityModifier entitiesToUpdate);
    }
}
