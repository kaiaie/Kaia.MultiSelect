using Kaia.Common.DataAccess;
using Kaia.MultiSelect.DataAccess.Contract;
using Kaia.MultiSelect.Domain;
using System.Data;

namespace Kaia.MultiSelect.DataAccess
{
    public class SupplierRepository : 
        DbRepositoryBase<Supplier, NewSupplier, SupplierModifier>, 
        ISupplierRepository
    {
        public SupplierRepository(IDbConnection connection, IDbTransaction transaction) :
            base(connection, transaction)
        {

        }
    }
}
