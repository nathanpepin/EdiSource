using EdiSource._271_5010.Loop2000C_SubscriberLevel.Loop2100C_SubscriberName.Segments;
using EdiSource._271_5010.Loop2000C_SubscriberLevel.Loop2100C.Loop2110C;

namespace EdiSource._271_5010.Loop2000C_SubscriberLevel.Loop2100C;

[LoopGenerator<_271_5010_Loop2000C_SubscriberLevel, _271_5010_Loop2100C_SubscriberName, _271_5010_Loop2100C_NM1_SubscriberName>]
public sealed partial class _271_5010_Loop2100C_SubscriberName : IValidatable
{
    [SegmentHeader] public _271_5010_Loop2100C_NM1_SubscriberName NM1_SubscriberName { get; set; } = null!;

    [Segment] public _271_5010_Loop2100C_N3_SubscriberAddress? N3_SubscriberAddress { get; set; }
    [Segment] public _271_5010_Loop2100C_N4_SubscriberGeographicLocation? N4_SubscriberGeographicLocation { get; set; }
    [Segment] public _271_5010_Loop2100C_DMG_SubscriberDemographicInfo? DMG_SubscriberDemographicInfo { get; set; }
    [Segment] public _271_5010_Loop2100C_INS_SubscriberRelationship? INS_SubscriberRelationship { get; set; }
    [Segment] public _271_5010_Loop2100C_HI_SubscriberHealthCareCodeInfo? HI_SubscriberHealthCareCodeInfo { get; set; }
    [Segment] public _271_5010_Loop2100C_MP_SubscriberMilitaryPersonnelInfo? MP_SubscriberMilitaryPersonnelInfo { get; set; }

    [SegmentList] public SegmentList<_271_5010_Loop2100C_REF_SocialSecurityNumber> REF_SocialSecurityNumbers { get; set; } = [];
    [SegmentList] public SegmentList<_271_5010_Loop2100C_REF_GroupNumber> REF_GroupNumbers { get; set; } = [];
    [SegmentList] public SegmentList<_271_5010_Loop2100C_REF_PlanNumber> REF_PlanNumbers { get; set; } = [];

    [LoopList] public LoopList<_271_5010_Loop2110C_SubscriberEligibilityOrBenefitInfo> Loop2110C_SubscriberEligibilityOrBenefitInfos { get; set; } = [];

    public IEnumerable<ValidationMessage> Validate()
    {
        if (NM1_SubscriberName == null)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "NM1 segment is required");
    }
}