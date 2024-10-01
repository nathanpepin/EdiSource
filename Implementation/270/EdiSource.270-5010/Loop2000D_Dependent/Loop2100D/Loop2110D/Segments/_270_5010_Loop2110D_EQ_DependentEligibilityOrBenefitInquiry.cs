namespace EdiSource._270_5010.Loop2000D_Dependent.Loop2100D.Loop2110D.Segments;

[SegmentGenerator<_270_5010_Loop2110D_DependentEligibilityBenefitInquiry>("EQ")]
public sealed partial class _270_5010_Loop2110D_EQ_DependentEligibilityOrBenefitInquiry
{
    public string? ServiceTypeCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string? CoverageLevel
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string? InsuranceTypeCode
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string? PlanNetworkIdentificationCode
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }
}