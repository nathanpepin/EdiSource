namespace EdiSource._271_5010.Loop2000D_DependentLevel.Loop2100D_DependentName.Loop2110D_DependentEligibilityOrBenefitInfo.Loop2120D_BenefitRelatedEntityGrouping.Loop2120D_BenefitRelatedEntity.Segments;

[SegmentGenerator<_271_5010_Loop2120D_BenefitRelatedEntity>("PRV")]
public partial class _271_5010_Loop2120D_PRV_BenefitEntityProviderInfo
{
    public string? ProviderCode { get => GetCompositeElement(1); set => SetCompositeElement(value, 1); }
    public string? ReferenceIdentificationQualifier { get => GetCompositeElement(2); set => SetCompositeElement(value, 2); }
    public string? BenefitRelatedEntityProviderTaxonomyCode { get => GetCompositeElement(3); set => SetCompositeElement(value, 3); }
}
