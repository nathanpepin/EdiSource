namespace EdiSource._271_5010.Loop2000D_DependentLevel.Loop2100D_DependentName.Loop2110D_DependentEligibilityOrBenefitInfo.Segments;

[SegmentGenerator<_271_5010_Loop2110D_DependentEligibilityOrBenefitInfo>("PRV")]
public partial class _271_5010_Loop2110D_PRV_ProviderInfo
{
    public string? ProviderCode { get => GetCompositeElement(1); set => SetCompositeElement(value, 1); }
    public string? ReferenceIdentificationQualifier { get => GetCompositeElement(2); set => SetCompositeElement(value, 2); }
    public string? ProviderTaxonomyCode { get => GetCompositeElement(3); set => SetCompositeElement(value, 3); }
}
