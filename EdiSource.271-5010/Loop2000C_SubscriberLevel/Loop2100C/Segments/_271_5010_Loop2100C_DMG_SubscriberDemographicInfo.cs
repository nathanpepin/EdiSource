namespace EdiSource._271_5010.Loop2000C_SubscriberLevel.Loop2100C_SubscriberName.Segments;

[SegmentGenerator<_271_5010_Loop2100C_SubscriberName>("DMG")]
public partial class _271_5010_Loop2100C_DMG_SubscriberDemographicInfo
{
    public string? DateTimePeriodFormatQualifier { get => GetCompositeElement(1); set => SetCompositeElement(value, 1); }
    public DateOnly? DateOfBirth { get => this.GetDateOnly(2); set => this.SetDateOnly(value, 2); }
    public string? GenderCode { get => GetCompositeElement(3); set => SetCompositeElement(value, 3); }
    public string? MaritalStatusCode { get => GetCompositeElement(4); set => SetCompositeElement(value, 4); }

    public string? RaceOrEthnicityCode1 { get => GetCompositeElement(5, 1); set => SetCompositeElement(value, 5, 1); }
    public string? RaceOrEthnicityCode2 { get => GetCompositeElement(5, 2); set => SetCompositeElement(value, 5, 2); }
    public string? RaceOrEthnicityCode3 { get => GetCompositeElement(5, 3); set => SetCompositeElement(value, 5, 3); }

    public string? CitizenshipStatusCode { get => GetCompositeElement(6); set => SetCompositeElement(value, 6); }
    public string? CountryCode { get => GetCompositeElement(7); set => SetCompositeElement(value, 7); }
    public string? BasisOfVerificationCode { get => GetCompositeElement(8); set => SetCompositeElement(value, 8); }
    public decimal? Quantity { get => this.GetDecimal(9); set => this.SetDecimal(value, 9); }
    public string? CodeListQualifierCode { get => GetCompositeElement(10); set => SetCompositeElement(value, 10); }
    public string? IndustryCode { get => GetCompositeElement(11); set => SetCompositeElement(value, 11); }
}
