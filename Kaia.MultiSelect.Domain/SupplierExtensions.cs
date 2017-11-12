using Kaia.Common.DataAccess;
using System.Collections.Generic;
using System.Linq;

namespace Kaia.MultiSelect.Domain
{
    /// <summary>
    /// Represents the changes to one ore more suppliers
    /// </summary>
    public static class SupplierExtensions
    {
        public static SupplierModifier GetModifier(this Supplier supplier)
        {
            return new SupplierModifier(new List<long> { supplier.SupplierId })
            {
                SupplierName = new UpdatableField<string>(supplier.SupplierName),
                Status = new UpdatableField<long>(supplier.Status),
                City = new UpdatableField<string>(supplier.City)
            };
        }

        public static SupplierModifier GetModifier(this IEnumerable<Supplier> suppliers)
        {
            Indeterminate<string> supplierName = null;
            Indeterminate<long> status = null;
            Indeterminate<string> city = null;
            var ids = new List<long>();
            foreach (var supplier in suppliers)
            {
                ids.Add(supplier.SupplierId);
                if (supplierName == null)
                {
                    supplierName = 
                        new Indeterminate<string>(supplier.SupplierName);
                }
                else
                {
                    supplierName.Value = supplier.SupplierName;
                }
                if (status == null)
                {
                    status = new Indeterminate<long>(supplier.Status);
                }
                else
                {
                    status.Value = supplier.Status;
                }
                if (city == null)
                {
                    city = new Indeterminate<string>(supplier.City);
                }
                else
                {
                    city.Value = supplier.City;
                }
            }
            var result = new SupplierModifier(ids)
            {
                SupplierName = new UpdatableField<string>(supplierName, 
                    ids.Count > 1), // Can only update if modifying a single entity
                Status = new UpdatableField<long>(status),
                City = new UpdatableField<string>(city)
            };
            return result;
        }
    }
}
