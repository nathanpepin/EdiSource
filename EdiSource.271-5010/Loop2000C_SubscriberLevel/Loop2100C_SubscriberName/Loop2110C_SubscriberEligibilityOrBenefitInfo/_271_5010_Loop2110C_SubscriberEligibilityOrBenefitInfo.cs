using EdiSource._271_5010.Loop2000C_SubscriberLevel.Loop2100C_SubscriberName.Loop2110C_SubscriberEligibilityOrBenefitInfo.Loop2120C_BenefitRelatedEntityGrouping;
using EdiSource._271_5010.Loop2000C_SubscriberLevel.Loop2100C_SubscriberName.Loop2110C_SubscriberEligibilityOrBenefitInfo.Segments;

namespace EdiSource._271_5010.Loop2000C_SubscriberLevel.Loop2100C_SubscriberName.Loop2110C_SubscriberEligibilityOrBenefitInfo;

[LoopGenerator<_271_5010_Loop2100C_SubscriberName, _271_5010_Loop2110C_SubscriberEligibilityOrBenefitInfo, _271_5010_Loop2110C_EB_EligibilityOrBenefitInfo>]
public partial class _271_5010_Loop2110C_SubscriberEligibilityOrBenefitInfo : IValidatable
{
    [SegmentHeader] public _271_5010_Loop2110C_EB_EligibilityOrBenefitInfo EB_EligibilityOrBenefitInfo { get; set; } = null!;

    [Segment] public _271_5010_Loop2110C_HSD_HealthCareServicesDelivery? HSD_HealthCareServicesDelivery { get; set; }

    [SegmentList] public SegmentList<_271_5010_Loop2110C_REF_AdditionalSubscriberInfo> REF_AdditionalSubscriberInfos { get; set; } = [];
    [SegmentList] public SegmentList<_271_5010_Loop2110C_DTP_PlanBeginDate> DTP_PlanBeginDates { get; set; } = [];
    [SegmentList] public SegmentList<_271_5010_Loop2110C_DTP_PlanEndDate> DTP_PlanEndDates { get; set; } = [];
    [SegmentList] public SegmentList<_271_5010_Loop2110C_DTP_EligibilityBeginDate> DTP_EligibilityBeginDates { get; set; } = [];
    [SegmentList] public SegmentList<_271_5010_Loop2110C_DTP_EligibilityEndDate> DTP_EligibilityEndDates { get; set; } = [];
    [SegmentList] public SegmentList<_271_5010_Loop2110C_AAA_RequestValidation> AAA_RequestValidations { get; set; } = [];
    [SegmentList] public SegmentList<_271_5010_Loop2110C_MSG_MessageText> MSG_MessageTexts { get; set; } = [];

    [Segment] public _271_5010_Loop2110C_PRV_ProviderInfo? PRV_ProviderInfo { get; set; }

    [LoopList] public LoopList<_271_5010_Loop2120C_BenefitRelatedEntityGrouping> Loop2120C_BenefitRelatedEntityGroupings { get; set; } = [];

    public IEnumerable<ValidationMessage> Validate()
    {
        if (EB_EligibilityOrBenefitInfo == null)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "EB segment is required");
    }
}
