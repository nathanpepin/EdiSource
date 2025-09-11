namespace EdiSource._270_5010.Loop2000D_Dependent.Loop2100D.Loop2110D.Segments;

[SegmentGenerator<_270_5010_Loop2110D_DependentEligibilityBenefitInquiry>("III")]
public sealed partial class _270_5010_Loop2110D_III_DependentEligibilityOrBenefitAdditionalInfo
{
    public string? CodeListQualifierCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string? IndustryCode
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string? CodeCategory
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string? FreeFormMessage
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }

    public decimal? Quantity
    {
        get => this.GetDecimal(5);
        set => this.SetDecimal(value, 5);
    }

    public string? CompositeUnitOfMeasure
    {
        get => GetCompositeElement(6);
        set => SetCompositeElement(value, 6);
    }

    public string? SurfaceLayerPositionCode
    {
        get => GetCompositeElement(7);
        set => SetCompositeElement(value, 7);
    }

    public string? SurfaceLayerPositionCode2
    {
        get => GetCompositeElement(8);
        set => SetCompositeElement(value, 8);
    }

    public string? SurfaceLayerPositionCode3
    {
        get => GetCompositeElement(9);
        set => SetCompositeElement(value, 9);
    }
}