using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Standard.Loops;
using EdiSource.Domain.Validation.Factory;
using EdiSource.Domain.Validation.Validator;

namespace EdiSource.Domain.Standard.Segments;

public sealed class GS : Segment, IEdi<FunctionalGroup>, ISegmentIdentifier<GS>
{
    public FunctionalGroup? Parent { get; set; }

    public string E06GroupControlNumber
    {
        get => GetCompositeElement(6);
        set => SetCompositeElement(value, 6);
    }

    public static EdiId EdiId { get; } = new("GS");
}