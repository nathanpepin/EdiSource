//HintName: TransactionSet.EdiElement.g.cs
#nullable enable
using EdiSource.Domain.Separator;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Domain.Loop;
using EdiSource.Loops;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Test
{
    public partial class TransactionSet
    {
        public List<IEdi?> EdiItems => new List<IEdi?> { ST, DTP, REFs, Loop2000, Loop2100s, SE, SE };
    }
}
