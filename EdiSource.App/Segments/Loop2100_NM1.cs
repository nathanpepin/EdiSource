using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Loops;

namespace EdiSource.Segments;

public class Loop2100_NM1 : Segment, ISegment<Loop2100>, ISegmentIdentifier<Loop2100_NM1>
{
    public Loop2100? Parent { get; }

    public static (string Primary, string? Secondary) EdiId => ("NM1", null);
}