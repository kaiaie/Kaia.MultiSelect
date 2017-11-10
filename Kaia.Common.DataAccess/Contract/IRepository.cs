using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Common.DataAccess.Contract
{
    public interface IRepository<TEntity, TNewEntity, TEntityModifier>
        where TEntity : class
        where TEntityModifier : IEntityModifier
    {
        IEnumerable<TEntity> GetAll();


        TEntity Get(long id);

        IEnumerable<TEntity> Get(IEnumerable<long> ids);

        void Create(TNewEntity newEntity);

        void Modify(TEntityModifier entitiesToUpdate);
    }
}
