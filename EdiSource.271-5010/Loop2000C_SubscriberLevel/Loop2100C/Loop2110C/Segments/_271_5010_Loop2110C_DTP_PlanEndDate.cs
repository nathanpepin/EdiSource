namespace EdiSource._271_5010.Loop2000C_SubscriberLevel.Loop2100C_SubscriberName.Loop2110C_SubscriberEligibilityOrBenefitInfo.Segments;

[SegmentGenerator<_271_5010_Loop2110C_SubscriberEligibilityOrBenefitInfo>("DTP", "292")]
public partial class _271_5010_Loop2110C_DTP_PlanEndDate
{
    public string? DateTimeQualifier { get => GetCompositeElement(1); set => SetCompositeElement(value, 1); }
    public string? DateTimePeriodFormatQualifier { get => GetCompositeElement(2); set => SetCompositeElement(value, 2); }
    public DateOnly? PlanEndDate { get => this.GetDateOnly(3); set => this.SetDateOnly(value, 3); }
}
