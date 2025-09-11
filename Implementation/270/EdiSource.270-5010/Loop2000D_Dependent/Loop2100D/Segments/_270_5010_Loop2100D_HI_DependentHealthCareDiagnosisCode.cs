namespace EdiSource._270_5010.Loop2000D_Dependent.Loop2100D.Segments;

[SegmentGenerator<_270_5010_Loop2100D_DependentName>("HI")]
public sealed partial class _270_5010_Loop2100D_HI_DependentHealthCareDiagnosisCode
{
    public string? CodeListQualifierCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string? IndustryCode
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }
}