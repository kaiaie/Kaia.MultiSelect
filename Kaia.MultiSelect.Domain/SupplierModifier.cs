using Kaia.Common.DataAccess.Contract;
using System.Collections.Generic;
using Kaia.Common.DataAccess;
using Kaia.Common.DataAccess.DataAnnotations;

namespace Kaia.MultiSelect.Domain
{
    [Entity(typeof(Supplier))]
    public sealed class SupplierModifier : IEntityModifier
    {
        private List<long> _ids;

        public IReadOnlyList<long> Ids
        {
            get
            {
                return _ids.AsReadOnly();
            }
        }

        public ModificationType ModificationType { get; set; }

        public UpdatableField<string> SupplierName { get; set; }

        public UpdatableField<long> Status { get; set; }

        public UpdatableField<string> City { get; set; }


        public SupplierModifier(List<long> ids)
        {
            _ids = ids;
        }
    }
}
