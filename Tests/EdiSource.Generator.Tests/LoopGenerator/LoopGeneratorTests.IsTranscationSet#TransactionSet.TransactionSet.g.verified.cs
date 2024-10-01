//HintName: TransactionSet.TransactionSet.g.cs
#nullable enable
using EdiSource.Domain.Separator;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Validation.SourceGeneration;
using EdiSource.Domain.Validation.Data;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Test
{
    public partial class TransactionSet : ITransactionSet<TransactionSet, TS_ST>
    {
        ISegment ITransactionSet.ST => ST;

        ISegment ITransactionSet.SE => SE;
        public static TransactionSetDefinition Definition { get; } = id =>
        {
            if (EdiId.Primary != id.Item1 || (EdiId.Secondary is not null && EdiId.Secondary != id.Item2))
            {
                return null;
            }

            return (segmentReader, parent) => InitializeAsync(segmentReader, parent).ContinueWith(ILoop (x) => x.Result);
        };
    }
}