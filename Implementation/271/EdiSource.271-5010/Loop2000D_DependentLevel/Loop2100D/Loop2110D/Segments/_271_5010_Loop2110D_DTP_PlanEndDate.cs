namespace EdiSource._271_5010.Loop2000D_DependentLevel.Loop2100D.Loop2110D.Segments;

[SegmentGenerator<_271_5010_Loop2110D_DependentEligibilityOrBenefitInfo>("DTP", "292")]
public sealed partial class _271_5010_Loop2110D_DTP_PlanEndDate
{
    public string? DateTimeQualifier
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string? DateTimePeriodFormatQualifier
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public DateOnly? PlanEndDate
    {
        get => this.GetDateOnly(3);
        set => this.SetDateOnly(value, 3);
    }
}