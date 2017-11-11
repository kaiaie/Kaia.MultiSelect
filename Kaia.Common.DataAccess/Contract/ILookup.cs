namespace Kaia.Common.DataAccess.Contract
{
    /// <summary>
    /// Interface implemented by entities that can be used as lookups (e.g., 
    /// displayed in a dropdown list)
    /// </summary>
    public interface ILookup
    {
        long Id { get; }

        string Text { get; }
    }
}
