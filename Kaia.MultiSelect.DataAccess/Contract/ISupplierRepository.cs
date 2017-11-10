using Kaia.Common.DataAccess.Contract;
using Kaia.MultiSelect.Domain;

namespace Kaia.MultiSelect.DataAccess.Contract
{
    public interface ISupplierRepository : 
        IRepository<Supplier, NewSupplier, SupplierModifier>
    {
    }
}
