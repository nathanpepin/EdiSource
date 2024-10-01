namespace EdiSource._271_5010.Loop2000C_SubscriberLevel.Loop2100C.Loop2110C.Segments;

[SegmentGenerator<_271_5010_Loop2110C_SubscriberEligibilityOrBenefitInfo>("DTP", "346")]
public sealed partial class _271_5010_Loop2110C_DTP_EligibilityBeginDate
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

    public DateOnly? EligibilityBeginDate
    {
        get => this.GetDateOnly(3);
        set => this.SetDateOnly(value, 3);
    }
}