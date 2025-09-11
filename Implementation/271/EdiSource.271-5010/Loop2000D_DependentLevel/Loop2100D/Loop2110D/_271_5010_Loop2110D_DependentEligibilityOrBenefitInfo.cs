
using EdiSource._271_5010.Loop2000D_DependentLevel.Loop2100D.Loop2110D.Loop2120D;
using EdiSource._271_5010.Loop2000D_DependentLevel.Loop2100D.Loop2110D.Segments;

namespace EdiSource._271_5010.Loop2000D_DependentLevel.Loop2100D.Loop2110D;

[LoopGenerator<_271_5010_Loop2100D_DependentName, _271_5010_Loop2110D_DependentEligibilityOrBenefitInfo, _271_5010_Loop2110D_EB_EligibilityOrBenefitInfo>]
public partial class _271_5010_Loop2110D_DependentEligibilityOrBenefitInfo : IValidatable
{
    [SegmentHeader] public _271_5010_Loop2110D_EB_EligibilityOrBenefitInfo EB_EligibilityOrBenefitInfo { get; set; } = null!;

    [Segment] public _271_5010_Loop2110D_HSD_HealthCareServicesDelivery? HSD_HealthCareServicesDelivery { get; set; }

    [SegmentList] public SegmentList<_271_5010_Loop2110D_REF_AdditionalDependentInfo> REF_AdditionalDependentInfos { get; set; } = [];
    [SegmentList] public SegmentList<_271_5010_Loop2110D_DTP_PlanBeginDate> DTP_PlanBeginDates { get; set; } = [];
    [SegmentList] public SegmentList<_271_5010_Loop2110D_DTP_PlanEndDate> DTP_PlanEndDates { get; set; } = [];
    [SegmentList] public SegmentList<_271_5010_Loop2110D_DTP_EligibilityBeginDate> DTP_EligibilityBeginDates { get; set; } = [];
    [SegmentList] public SegmentList<_271_5010_Loop2110D_DTP_EligibilityEndDate> DTP_EligibilityEndDates { get; set; } = [];
    [SegmentList] public SegmentList<_271_5010_Loop2110D_AAA_RequestValidation> AAA_RequestValidations { get; set; } = [];
    [SegmentList] public SegmentList<_271_5010_Loop2110D_MSG_MessageText> MSG_MessageTexts { get; set; } = [];

    [Segment] public _271_5010_Loop2110D_PRV_ProviderInfo? PRV_ProviderInfo { get; set; }

    [LoopList] public LoopList<_271_5010_Loop2120D_BenefitRelatedEntityGrouping> Loop2120D_BenefitRelatedEntityGroupings { get; set; } = [];

    public IEnumerable<ValidationMessage> Validate()
    {
        if (EB_EligibilityOrBenefitInfo == null)
            yield return ValidationFactory.Create((ILoop)this, ValidationSeverity.Critical, "EB segment is required");
    }
}
