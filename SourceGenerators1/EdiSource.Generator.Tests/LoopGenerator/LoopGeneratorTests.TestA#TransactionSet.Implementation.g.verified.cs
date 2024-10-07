//HintName: TransactionSet.Implementation.g.cs
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
    : ILoop, ILoop<TransactionSet>, ISegmentIdentifier<TransactionSet>, ISegmentIdentifier<TS_ST>, ILoopInitialize<TransactionSet, TransactionSet>
    {
        ILoop? ILoop.Parent => Parent;
        public TransactionSet? Parent { get => null; set {} }
        public static (string Primary, string? Secondary) EdiId => TS_ST.EdiId;
    }
}
