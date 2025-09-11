namespace EdiSource._270_5010.Loop2000D_Dependent.Loop2100D.Segments;

[SegmentGenerator<_270_5010_Loop2100D_DependentName>("N3")]
public sealed partial class _270_5010_Loop2100D_N3_DependentAddress
{
    public string? AddressLine1
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string? AddressLine2
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }
}