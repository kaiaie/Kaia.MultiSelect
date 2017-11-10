namespace Kaia.MultiSelect.Domain
{
    /// <summary>
    /// Used to gather values for a new Supplier
    /// </summary>
    public sealed class NewSupplier
    {
        public string SupplierName { get; set; }
        public long Status { get; set; }
        public string City { get; set; }
    }
}
