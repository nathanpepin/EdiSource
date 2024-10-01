using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Loops;

namespace EdiSource.Segments;

public class TS_ST : Segment, ISegment<TransactionSet>, ISegmentIdentifier<TS_ST>
{
    public TransactionSet? Parent { get; }

    public static (string Primary, string? Secondary) EdiId => ("ST", null);
}