using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Standard.Loops;

namespace EdiSource.Domain.Standard.Segments;

public sealed class IEA : Segment, ISegment<InterchangeEnvelope>, ISegmentIdentifier<IEA>
{
    public InterchangeEnvelope? Parent { get; }
    public static (string Primary, string? Secondary) EdiId { get; } = ("IEA", null);
}