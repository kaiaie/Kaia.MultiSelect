using Kaia.Common.DataAccess.Contract;
using Kaia.Common.DataAccess.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Kaia.MultiSelect.Domain
{
    /// <summary>
    /// Represents a Supplier in the database
    /// </summary>
    /// <remarks>
    /// This class is immutable. Call the GetModifier extension method to get 
    /// a modifiable object
    /// </remarks>
    public sealed class Supplier : ILookup
    {
        private readonly long _supplierId;
        private readonly string _supplierName;
        private readonly long _status;
        private readonly string _city;

        [Key]
        public long SupplierId { get { return _supplierId; } }
        [Unique]
        public string SupplierName { get { return _supplierName; } }
        public long Status { get { return _status; } }
        public string City { get { return _city; } }

        [Ignore]
        public long Id
        {
            get
            {
                return _supplierId;
            }
        }

        [Ignore]
        public string Text
        {
            get
            {
                return string.Format("{0} ({1})", _supplierName, _city);
            }
        }

        public Supplier(long supplierId, string supplierName, long status, string city)
        {
            _supplierId = supplierId;
            _supplierName = supplierName;
            _status = status;
            _city = city;
        }
    }
}
