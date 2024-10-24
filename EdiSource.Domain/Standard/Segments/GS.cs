using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Standard.Loops;

namespace EdiSource.Domain.Standard.Segments;

public sealed class GS : Segment, IEdi<FunctionalGroup>, ISegmentIdentifier<GS>
{
    public string E06GroupControlNumber
    {
        get => GetCompositeElement(6);
        set => SetCompositeElement(value, 6);
    }

    public FunctionalGroup? Parent { get; set; }

    public static EdiId EdiId { get; } = new("GS");
}