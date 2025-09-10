namespace EdiSource._271_5010.Loop2000C_SubscriberLevel.Loop2100C_SubscriberName.Loop2110C_SubscriberEligibilityOrBenefitInfo.Segments;

[SegmentGenerator<_271_5010_Loop2110C_SubscriberEligibilityOrBenefitInfo>("PRV")]
public partial class _271_5010_Loop2110C_PRV_ProviderInfo
{
    public string? ProviderCode { get => GetCompositeElement(1); set => SetCompositeElement(value, 1); }
    public string? ReferenceIdentificationQualifier { get => GetCompositeElement(2); set => SetCompositeElement(value, 2); }
    public string? ProviderTaxonomyCode { get => GetCompositeElement(3); set => SetCompositeElement(value, 3); }
}
