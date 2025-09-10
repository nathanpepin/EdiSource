namespace EdiSource._271_5010.TransactionSet.Segments;

[SegmentGenerator<_271_5010_EligibilityBenefitResponse>("ST", "271", null, "005010X279A1")]
public partial class _271_5010_ST_TransactionSetHeader
{
    public string? TransactionSetIdentifierCode { get => GetCompositeElement(1); set => SetCompositeElement(value, 1); }
    public string? TransactionSetControlNumber { get => GetCompositeElement(2); set => SetCompositeElement(value, 2); }
    public string? ImplementationConventionReference { get => GetCompositeElement(3); set => SetCompositeElement(value, 3); }
}
