namespace EdiSource._271_5010.Loop2000D_DependentLevel.Loop2100D_DependentName.Loop2110D_DependentEligibilityOrBenefitInfo.Segments;

[SegmentGenerator<_271_5010_Loop2110D_DependentEligibilityOrBenefitInfo>("DTP", "291")]
public partial class _271_5010_Loop2110D_DTP_PlanBeginDate
{
    public string? DateTimeQualifier { get => GetCompositeElement(1); set => SetCompositeElement(value, 1); }
    public string? DateTimePeriodFormatQualifier { get => GetCompositeElement(2); set => SetCompositeElement(value, 2); }
    public DateOnly? PlanBeginDate { get => this.GetDateOnly(3); set => this.SetDateOnly(value, 3); }
}
