using EdiSource._271_5010.Loop2000D_DependentLevel.Loop2100D_DependentName;

namespace EdiSource._271_5010.Loop2000D_DependentLevel.Loop2100D.Segments;

[SegmentGenerator<_271_5010_Loop2100D_DependentName>("N4")]
public sealed partial class _271_5010_Loop2100D_N4_DependentGeographicLocation
{
    public string? CityName { get => GetCompositeElement(1); set => SetCompositeElement(value, 1); }
    public string? StateOrProvinceCode { get => GetCompositeElement(2); set => SetCompositeElement(value, 2); }
    public string? PostalCode { get => GetCompositeElement(3); set => SetCompositeElement(value, 3); }
    public string? CountryCode { get => GetCompositeElement(4); set => SetCompositeElement(value, 4); }
    public string? LocationQualifier { get => GetCompositeElement(5); set => SetCompositeElement(value, 5); }
    public string? LocationIdentifier { get => GetCompositeElement(6); set => SetCompositeElement(value, 6); }
    public string? CountrySubdivisionCode { get => GetCompositeElement(7); set => SetCompositeElement(value, 7); }
}
