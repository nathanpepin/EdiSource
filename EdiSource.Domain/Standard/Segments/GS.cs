using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Standard.Loops;

namespace EdiSource.Domain.Standard.Segments;

public sealed class GS : Segment, ISegment<FunctionalGroup>, ISegmentIdentifier<GS>
{
    public new FunctionalGroup? Parent { get; }

    public string E06GroupControlNumber
    {
        get => GetCompositeElement(6);
        set => SetCompositeElement(value, 6);
    }

    public static (string Primary, string? Secondary) EdiId { get; } = ("GS", null);
}