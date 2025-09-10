namespace EdiSource._271_5010.Loop2000C_SubscriberLevel.Loop2100C_SubscriberName.Loop2110C_SubscriberEligibilityOrBenefitInfo.Loop2120C_BenefitRelatedEntityGrouping.Loop2120C_BenefitRelatedEntity.Segments;

[SegmentGenerator<_271_5010_Loop2120C_BenefitRelatedEntity>("PRV")]
public partial class _271_5010_Loop2120C_PRV_BenefitRelatedEntityProviderInfo
{
    public string? ProviderCode { get => GetCompositeElement(1); set => SetCompositeElement(value, 1); }
    public string? ReferenceIdentificationQualifier { get => GetCompositeElement(2); set => SetCompositeElement(value, 2); }
    public string? BenefitRelatedEntityProviderTaxonomyCode { get => GetCompositeElement(3); set => SetCompositeElement(value, 3); }
}
