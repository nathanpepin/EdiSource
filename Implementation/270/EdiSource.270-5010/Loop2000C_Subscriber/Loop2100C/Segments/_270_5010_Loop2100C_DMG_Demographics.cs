namespace EdiSource._270_5010.Loop2000C_Subscriber.Loop2100C.Segments;

[SegmentGenerator<_270_5010_Loop2100C_SubscriberName>("DMG")]
public sealed partial class _270_5010_Loop2100C_DMG_Demographics
{
    public string? DateTimePeriodFormatQualifier
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public DateOnly? DateOfBirth
    {
        get => this.GetDateOnly(2);
        set => this.SetDateOnly(value, 2);
    }

    public string? GenderCode
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string? MaritalStatusCode
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }

    public string? RaceOrEthnicityCode
    {
        get => GetCompositeElement(5);
        set => SetCompositeElement(value, 5);
    }
}