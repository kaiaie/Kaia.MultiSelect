﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kaia.Common.DataAccess.Contract
{
    public interface IEntityModifier
    {
        ModificationType ModificationType { get; }

        IReadOnlyList<long> Ids { get; }
    }
}
