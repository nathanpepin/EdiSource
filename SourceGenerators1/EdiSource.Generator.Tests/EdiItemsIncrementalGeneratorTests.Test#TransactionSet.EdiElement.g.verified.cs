//HintName: TransactionSet.EdiElement.g.cs
using System.Collections.Generic;
using EdiSource.Domain.Identifiers;

namespace EdiSource.Loops
{
    public partial class TransactionSet : ILoop<TransactionSet>, ISegmentIdentifier<TransactionSet>, ISegmentIdentifier<TS_ST>
    {
        ILoop? ILoop.Parent => Parent;
        public TransactionSet? Parent { get; set; } = null;
        public static (string Primary, string? Secondary) EdiId => TS_ST.EdiId;
    }
}