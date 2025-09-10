namespace EdiSource._271_5010.Loop2000C_SubscriberLevel.Loop2100C_SubscriberName.Loop2110C_SubscriberEligibilityOrBenefitInfo.Segments;

[SegmentGenerator<_271_5010_Loop2110C_SubscriberEligibilityOrBenefitInfo>("AAA")]
public partial class _271_5010_Loop2110C_AAA_RequestValidation
{
    public string? ValidRequestIndicator { get => GetCompositeElement(1); set => SetCompositeElement(value, 1); }
    public string? AgencyQualifierCode { get => GetCompositeElement(2); set => SetCompositeElement(value, 2); }
    public string? RejectReasonCode { get => GetCompositeElement(3); set => SetCompositeElement(value, 3); }
    public string? FollowUpActionCode { get => GetCompositeElement(4); set => SetCompositeElement(value, 4); }
}
