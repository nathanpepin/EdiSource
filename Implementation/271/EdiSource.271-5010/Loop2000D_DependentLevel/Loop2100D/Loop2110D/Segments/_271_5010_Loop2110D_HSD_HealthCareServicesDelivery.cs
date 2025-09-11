using EdiSource._271_5010.Loop2000D_DependentLevel.Loop2100D_DependentName.Loop2110D_DependentEligibilityOrBenefitInfo;

namespace EdiSource._271_5010.Loop2000D_DependentLevel.Loop2100D.Loop2110D.Segments;

[SegmentGenerator<_271_5010_Loop2110D_DependentEligibilityOrBenefitInfo>("HSD")]
public sealed partial class _271_5010_Loop2110D_HSD_HealthCareServicesDelivery
{
    public string? QuantityQualifier { get => GetCompositeElement(1); set => SetCompositeElement(value, 1); }
    public decimal? Quantity { get => SegmentExtensions.GetDecimal(this, 2); set => this.SetDecimal(value, 2); }
    public string? UnitOrBasisForMeasurementCode { get => GetCompositeElement(3); set => SetCompositeElement(value, 3); }
    public decimal? SampleSelectionModulus { get => SegmentExtensions.GetDecimal(this, 4); set => this.SetDecimal(value, 4); }
    public string? TimePeriodQualifier { get => GetCompositeElement(5); set => SetCompositeElement(value, 5); }
    public int? PeriodCount { get => SegmentExtensions.GetInt(this, 6); set => this.SetInt(value, 6); }
    public string? DeliveryFrequencyCode { get => GetCompositeElement(7); set => SetCompositeElement(value, 7); }
    public string? DeliveryPatternTimeCode { get => GetCompositeElement(8); set => SetCompositeElement(value, 8); }
}
