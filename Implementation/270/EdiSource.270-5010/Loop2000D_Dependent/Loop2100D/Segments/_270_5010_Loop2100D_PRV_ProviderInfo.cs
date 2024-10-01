namespace EdiSource._270_5010.Loop2000D_Dependent.Loop2100D.Segments;

[SegmentGenerator<_270_5010_Loop2100D_DependentName>("PRV")]
public sealed partial class _270_5010_Loop2100D_PRV_ProviderInfo
{
    public string? ProviderCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string? ReferenceIdentificationQualifier
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string? ReferenceIdentification
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }
}