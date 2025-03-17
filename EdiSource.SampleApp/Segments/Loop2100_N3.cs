using EdiSource.Domain.SourceGeneration;

namespace EdiSource.Segments;

[SegmentGenerator<Loops.Loop2100>("N3")]
public partial class Loop2100_N3
{
    public string AddressLine1
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string? AddressLine2
    {
        get => GetCompositeElementOrNull(2);
        set => value?.Do(x => SetCompositeElement(x, 2));
    }
}