using EdiSource._271_5010.Loop2000D_DependentLevel.Loop2100D_DependentName.Loop2110D_DependentEligibilityOrBenefitInfo;

namespace EdiSource._271_5010.Loop2000D_DependentLevel.Loop2100D.Loop2110D.Segments;

[SegmentGenerator<_271_5010_Loop2110D_DependentEligibilityOrBenefitInfo>("AAA")]
public sealed partial class _271_5010_Loop2110D_AAA_RequestValidation
{
    public string? ValidRequestIndicator { get => GetCompositeElement(1); set => SetCompositeElement(value, 1); }
    public string? AgencyQualifierCode { get => GetCompositeElement(2); set => SetCompositeElement(value, 2); }
    public string? RejectReasonCode { get => GetCompositeElement(3); set => SetCompositeElement(value, 3); }
    public string? FollowUpActionCode { get => GetCompositeElement(4); set => SetCompositeElement(value, 4); }
}
