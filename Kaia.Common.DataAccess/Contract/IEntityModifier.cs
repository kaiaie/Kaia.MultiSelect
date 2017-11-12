using System.Collections.Generic;

namespace Kaia.Common.DataAccess.Contract
{
    public interface IEntityModifier
    {
        ModificationType ModificationType { get; set; }

        IReadOnlyList<long> Ids { get; }
    }
}
