
namespace EdiSource._271_5010.Loop2000D_DependentLevel.Loop2100D.Loop2110D.Segments;

[SegmentGenerator<_271_5010_Loop2110D_DependentEligibilityOrBenefitInfo>("EB")]
public sealed partial class _271_5010_Loop2110D_EB_EligibilityOrBenefitInfo
{
    public string? EligibilityOrBenefitInformationCode { get => GetCompositeElement(1); set => SetCompositeElement(value, 1); }
    public string? CoverageLevelCode { get => GetCompositeElement(2); set => SetCompositeElement(value, 2); }

    public string? ServiceTypeCode1 { get => GetCompositeElement(3, 1); set => SetCompositeElement(value, 3, 1); }
    public string? ServiceTypeCode2 { get => GetCompositeElement(3, 2); set => SetCompositeElement(value, 3, 2); }
    public string? ServiceTypeCode3 { get => GetCompositeElement(3, 3); set => SetCompositeElement(value, 3, 3); }
    public string? ServiceTypeCode4 { get => GetCompositeElement(3, 4); set => SetCompositeElement(value, 3, 4); }

    public string? InsuranceTypeCode { get => GetCompositeElement(4); set => SetCompositeElement(value, 4); }
    public string? PlanCoverageDescription { get => GetCompositeElement(5); set => SetCompositeElement(value, 5); }
    public string? TimePeriodQualifier { get => GetCompositeElement(6); set => SetCompositeElement(value, 6); }
    public decimal? MonetaryAmount { get => SegmentExtensions.GetDecimal(this, 7); set => this.SetDecimal(value, 7); }
    public decimal? PercentageAsDecimal { get => SegmentExtensions.GetDecimal(this, 8); set => this.SetDecimal(value, 8); }
    public string? QuantityQualifier { get => GetCompositeElement(9); set => SetCompositeElement(value, 9); }
    public decimal? Quantity { get => SegmentExtensions.GetDecimal(this, 10); set => this.SetDecimal(value, 10); }
    public string? AuthorizationOrCertificationIndicator { get => GetCompositeElement(11); set => SetCompositeElement(value, 11); }
    public string? InPlanNetworkIndicator { get => GetCompositeElement(12); set => SetCompositeElement(value, 12); }

    public string? MedicalProcedureIdentifier1_CodeListQualifierCode { get => GetCompositeElement(13, 1); set => SetCompositeElement(value, 13, 1); }
    public string? MedicalProcedureIdentifier1_IndustryCode { get => GetCompositeElement(13, 2); set => SetCompositeElement(value, 13, 2); }
    public string? MedicalProcedureIdentifier2_CodeListQualifierCode { get => GetCompositeElement(14, 1); set => SetCompositeElement(value, 14, 1); }
    public string? MedicalProcedureIdentifier2_IndustryCode { get => GetCompositeElement(14, 2); set => SetCompositeElement(value, 14, 2); }
}
