using Kaia.Common.DataAccess;
using Kaia.MultiSelect.DataAccess.Contract;
using Kaia.MultiSelect.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
