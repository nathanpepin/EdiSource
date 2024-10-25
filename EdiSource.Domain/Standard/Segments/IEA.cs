using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Segments.Extensions;
using EdiSource.Domain.Standard.Loops.ISA;

namespace EdiSource.Domain.Standard.Segments;

public sealed class IEA : Segment, IEdi<InterchangeEnvelope>, ISegmentIdentifier<IEA>, IRefresh
{
    public int E01NumberOfFunctionalGroups
    {
        get => Parent is null
            ? this.GetIntRequired(1)
            : Parent.FunctionalGroups.Count
                .Do(x => this.SetInt(x, 1));
        set => this.SetInt(value, 1);
    }

    public string E02InterchangeControlNumber
    {
        get => Parent is null
            ? GetCompositeElement(2)
            : Parent.ISA.GetCompositeElement(13)
                .Do(x => SetCompositeElement(x, 2));
        set => SetCompositeElement(value, 2);
    }

    public InterchangeEnvelope? Parent { get; set; }
    public static EdiId EdiId { get; } = new("IEA");
    public void Refresh()
    {
        _ = E01NumberOfFunctionalGroups;
        _ = E02InterchangeControlNumber;
    }
}