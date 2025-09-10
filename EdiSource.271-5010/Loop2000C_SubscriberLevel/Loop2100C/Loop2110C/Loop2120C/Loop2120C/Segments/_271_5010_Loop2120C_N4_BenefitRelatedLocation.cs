namespace EdiSource._271_5010.Loop2000C_SubscriberLevel.Loop2100C_SubscriberName.Loop2110C_SubscriberEligibilityOrBenefitInfo.Loop2120C_BenefitRelatedEntityGrouping.Loop2120C_BenefitRelatedEntity.Segments;

[SegmentGenerator<_271_5010_Loop2120C_BenefitRelatedEntity>("N4")]
public partial class _271_5010_Loop2120C_N4_BenefitRelatedLocation
{
    public string? CityName { get => GetCompositeElement(1); set => SetCompositeElement(value, 1); }
    public string? StateOrProvinceCode { get => GetCompositeElement(2); set => SetCompositeElement(value, 2); }
    public string? PostalCode { get => GetCompositeElement(3); set => SetCompositeElement(value, 3); }
    public string? CountryCode { get => GetCompositeElement(4); set => SetCompositeElement(value, 4); }
    public string? LocationQualifier { get => GetCompositeElement(5); set => SetCompositeElement(value, 5); }
    public string? LocationIdentifier { get => GetCompositeElement(6); set => SetCompositeElement(value, 6); }
    public string? CountrySubdivisionCode { get => GetCompositeElement(7); set => SetCompositeElement(value, 7); }
}
