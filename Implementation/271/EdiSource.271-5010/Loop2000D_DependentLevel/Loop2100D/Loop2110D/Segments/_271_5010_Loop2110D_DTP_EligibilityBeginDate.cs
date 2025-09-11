using EdiSource._271_5010.Loop2000D_DependentLevel.Loop2100D_DependentName.Loop2110D_DependentEligibilityOrBenefitInfo;

namespace EdiSource._271_5010.Loop2000D_DependentLevel.Loop2100D.Loop2110D.Segments;

[SegmentGenerator<_271_5010_Loop2110D_DependentEligibilityOrBenefitInfo>("DTP", "346")]
public sealed partial class _271_5010_Loop2110D_DTP_EligibilityBeginDate
{
    public string? DateTimeQualifier { get => GetCompositeElement(1); set => SetCompositeElement(value, 1); }
    public string? DateTimePeriodFormatQualifier { get => GetCompositeElement(2); set => SetCompositeElement(value, 2); }
    public DateOnly? EligibilityBeginDate { get => SegmentExtensions.GetDateOnly(this, 3); set => this.SetDateOnly(value, 3); }
}
