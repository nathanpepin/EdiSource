namespace EdiSource._271_5010.Loop2000C_SubscriberLevel.Loop2100C.Loop2110C.Segments;

[SegmentGenerator<_271_5010_Loop2110C_SubscriberEligibilityOrBenefitInfo>("HSD")]
public sealed partial class _271_5010_Loop2110C_HSD_HealthCareServicesDelivery
{
    public string? QuantityQualifier
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public decimal? Quantity
    {
        get => this.GetDecimal(2);
        set => this.SetDecimal(value, 2);
    }

    public string? UnitOrBasisForMeasurementCode
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public decimal? SampleSelectionModulus
    {
        get => this.GetDecimal(4);
        set => this.SetDecimal(value, 4);
    }

    public string? TimePeriodQualifier
    {
        get => GetCompositeElement(5);
        set => SetCompositeElement(value, 5);
    }

    public int? PeriodCount
    {
        get => this.GetInt(6);
        set => this.SetInt(value, 6);
    }

    public string? DeliveryFrequencyCode
    {
        get => GetCompositeElement(7);
        set => SetCompositeElement(value, 7);
    }

    public string? DeliveryPatternTimeCode
    {
        get => GetCompositeElement(8);
        set => SetCompositeElement(value, 8);
    }
}