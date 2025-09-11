using EdiSource._270_5010.Loop2000D_Dependent.Loop2100D.Loop2110D.Segments;

namespace EdiSource._270_5010.Loop2000D_Dependent.Loop2100D.Loop2110D;

[LoopGenerator<_270_5010_Loop2100D_DependentName, _270_5010_Loop2110D_DependentEligibilityBenefitInquiry,
    _270_5010_Loop2110D_EQ_DependentEligibilityOrBenefitInquiry>]
public sealed partial class _270_5010_Loop2110D_DependentEligibilityBenefitInquiry : IValidatable
{
    [SegmentHeader] public _270_5010_Loop2110D_EQ_DependentEligibilityOrBenefitInquiry EQ_DependentEligibilityOrBenefitInquiry { get; set; } = null!;

    [SegmentList]
    public SegmentList<_270_5010_Loop2110D_III_DependentEligibilityOrBenefitAdditionalInfo> III_DependentEligibilityOrBenefitAdditionalInfos { get; set; } = [];

    [SegmentList] public SegmentList<_270_5010_Loop2110D_REF_DependentAdditionalInfo> REF_DependentAdditionalInfos { get; set; } = [];

    [SegmentList] public SegmentList<_270_5010_Loop2110D_DTP_DependentEligibilityBenefitDate> DTP_DependentEligibilityBenefitDates { get; set; } = [];

    public IEnumerable<ValidationMessage> Validate()
    {
#pragma warning disable CS8602
        if (EQ_DependentEligibilityOrBenefitInquiry == null)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "EQ segment is required");
#pragma warning restore CS8602
    }
}