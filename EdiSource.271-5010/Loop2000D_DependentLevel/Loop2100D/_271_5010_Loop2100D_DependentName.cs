using EdiSource._271_5010.Loop2000D_DependentLevel.Loop2100D_DependentName.Loop2110D_DependentEligibilityOrBenefitInfo;
using EdiSource._271_5010.Loop2000D_DependentLevel.Loop2100D_DependentName.Segments;

namespace EdiSource._271_5010.Loop2000D_DependentLevel.Loop2100D_DependentName;

[LoopGenerator<_271_5010_Loop2000D_DependentLevel, _271_5010_Loop2100D_DependentName, _271_5010_Loop2100D_NM1_DependentName>]
public partial class _271_5010_Loop2100D_DependentName : IValidatable
{
    [SegmentHeader] public _271_5010_Loop2100D_NM1_DependentName NM1_DependentName { get; set; } = null!;

    [Segment] public _271_5010_Loop2100D_N3_DependentAddress? N3_DependentAddress { get; set; }
    [Segment] public _271_5010_Loop2100D_N4_DependentGeographicLocation? N4_DependentGeographicLocation { get; set; }
    [Segment] public _271_5010_Loop2100D_DMG_DependentDemographicInfo? DMG_DependentDemographicInfo { get; set; }
    [Segment] public _271_5010_Loop2100D_INS_DependentRelationship? INS_DependentRelationship { get; set; }
    [Segment] public _271_5010_Loop2100D_HI_DependentHealthCareCodeInfo? HI_DependentHealthCareCodeInfo { get; set; }
    [Segment] public _271_5010_Loop2100D_MP_DependentMilitaryPersonnelInfo? MP_DependentMilitaryPersonnelInfo { get; set; }

    [SegmentList] public SegmentList<_271_5010_Loop2100D_REF_SocialSecurityNumber> REF_SocialSecurityNumbers { get; set; } = [];
    [SegmentList] public SegmentList<_271_5010_Loop2100D_REF_GroupNumber> REF_GroupNumbers { get; set; } = [];
    [SegmentList] public SegmentList<_271_5010_Loop2100D_REF_PlanNumber> REF_PlanNumbers { get; set; } = [];

    [LoopList] public LoopList<_271_5010_Loop2110D_DependentEligibilityOrBenefitInfo> Loop2110D_DependentEligibilityOrBenefitInfos { get; set; } = [];

    public IEnumerable<ValidationMessage> Validate()
    {
        if (NM1_DependentName == null)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "NM1 segment is required");
    }
}
