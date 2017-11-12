using Kaia.Common.DataAccess;
using Kaia.MultiSelect.DataAccess.Contract;
using System.Configuration;

namespace Kaia.MultiSelect.DataAccess
{
    public sealed class UnitOfWork : DbUnitOfWorkBase, IUnitOfWork
    {
        private ISupplierRepository _supplierRepository;

        public UnitOfWork(ConnectionStringSettings connectionString) : 
            base(connectionString)
        {
        }

        public UnitOfWork(string providerName, string connectionString) : 
            base(providerName, connectionString)
        {
        }

        public ISupplierRepository SupplierRepository
        {
            get
            {
                if (_supplierRepository == null)
                {
                    _supplierRepository = 
                        new SupplierRepository(Connection, Transaction);
                }
                return _supplierRepository;
            }
        }
    }
}
