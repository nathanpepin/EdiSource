namespace EdiSource._270_5010.Loop2000D_Dependent.Loop2100D.Segments;

[SegmentGenerator<_270_5010_Loop2100D_DependentName>("DMG")]
public sealed partial class _270_5010_Loop2100D_DMG_DependentDemographics
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

    public string? CitizenshipStatusCode
    {
        get => GetCompositeElement(6);
        set => SetCompositeElement(value, 6);
    }

    public string? CountryCode
    {
        get => GetCompositeElement(7);
        set => SetCompositeElement(value, 7);
    }

    public string? BasisOfVerificationCode
    {
        get => GetCompositeElement(8);
        set => SetCompositeElement(value, 8);
    }

    public decimal? Quantity
    {
        get => this.GetDecimal(9);
        set => this.SetDecimal(value, 9);
    }

    public string? CodeListQualifierCode
    {
        get => GetCompositeElement(10);
        set => SetCompositeElement(value, 10);
    }

    public string? IndustryCode
    {
        get => GetCompositeElement(11);
        set => SetCompositeElement(value, 11);
    }
}