namespace EdiSource._271_5010.Loop2000A_InformationSourceLevel.Loop2100A_InformationSourceName.Segments;

[SegmentGenerator<_271_5010_Loop2100A_InformationSourceName>("PRV")]
public partial class _271_5010_Loop2100A_PRV_InformationSourceProviderInfo
{
    public string? ProviderCode { get => GetCompositeElement(1); set => SetCompositeElement(value, 1); }
    public string? ReferenceIdentificationQualifier { get => GetCompositeElement(2); set => SetCompositeElement(value, 2); }
    public string? InformationSourceProviderTaxonomyCode { get => GetCompositeElement(3); set => SetCompositeElement(value, 3); }
}
