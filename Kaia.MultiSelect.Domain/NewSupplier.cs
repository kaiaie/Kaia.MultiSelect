using Kaia.Common.DataAccess.DataAnnotations;

namespace Kaia.MultiSelect.Domain
{
    /// <summary>
    /// Used to gather values for a new Supplier
    /// </summary>
    [Entity(typeof(Supplier))]
    public sealed class NewSupplier
    {
        public string SupplierName { get; set; }
        public long Status { get; set; }
        public string City { get; set; }
    }
}
