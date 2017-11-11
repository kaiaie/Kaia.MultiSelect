using System.Collections.Generic;

namespace Kaia.Common.DataAccess.Contract
{
    public interface IEntityModifier
    {
        ModificationType ModificationType { get; }

        IReadOnlyList<long> Ids { get; }
    }
}
