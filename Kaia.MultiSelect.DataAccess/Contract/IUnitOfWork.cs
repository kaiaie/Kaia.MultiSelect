using Kaia.MultiSelect.DataAccess.Contract;

namespace Kaia.MultiSelect.DataAccess.Contract
{
    public interface IUnitOfWork : Common.DataAccess.Contract.IUnitOfWork
    {
        ISupplierRepository SupplierRepository { get; }
    }
}
