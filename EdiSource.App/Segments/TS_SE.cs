using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Loops;

namespace EdiSource.Segments;

public class TS_SE : Segment, ISegment<TransactionSet>, ISegmentIdentifier<TS_SE>, IEdiId
{
    public TransactionSet? Parent { get; }

    public static (string Primary, string? Secondary) EdiId => ("SE", null);
}