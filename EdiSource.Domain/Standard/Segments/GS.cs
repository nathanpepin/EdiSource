using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Standard.Loops;

namespace EdiSource.Domain.Standard.Segments;

public sealed class GS : Segment, ISegment<FunctionalGroup>, ISegmentIdentifier<GS>
{
    public FunctionalGroup? Parent { get; }
    public static (string Primary, string? Secondary) EdiId { get; } = ("GS", null);
}