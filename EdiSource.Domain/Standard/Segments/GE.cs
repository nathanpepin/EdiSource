using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Standard.Loops;

namespace EdiSource.Domain.Standard.Segments;

public sealed class GE : Segment, ISegment<FunctionalGroup>, ISegmentIdentifier<GE>
{
    public new FunctionalGroup? Parent { get; }
    public static (string Primary, string? Secondary) EdiId { get; } = ("GE", null);
}