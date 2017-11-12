namespace Kaia.Common.DataAccess.Contract
{
    /// <summary>
    /// Indicates whether a value has been updated
    /// </summary>
    public interface IUpdatable
    {
        bool IsUpdatable { get; }

        bool IsUpdated { get; }
    }
}
