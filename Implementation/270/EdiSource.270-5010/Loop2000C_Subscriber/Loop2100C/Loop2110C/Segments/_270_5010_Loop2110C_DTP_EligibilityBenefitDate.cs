namespace EdiSource._270_5010.Loop2000C_Subscriber.Loop2100C.Loop2110C.Segments;

[SegmentGenerator<_270_5010_Loop2110C_EligibilityBenefitInquiry>("DTP")]
public sealed partial class _270_5010_Loop2110C_DTP_EligibilityBenefitDate
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

    public DateOnly? DateTimePeriod
    {
        get => this.GetDateOnly(3);
        set => this.SetDateOnly(value, 3);
    }
}