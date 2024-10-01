namespace EdiSource._270_5010.TransactionSet.Segments;

[SegmentGenerator<_270_5010_EligibilityBenefitInquiry>("ST", "270", null, "005010X279A1")]
public sealed partial class _270_5010_ST_TransactionSetHeader
{
    public string? TransactionSetIdentifierCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string? TransactionSetControlNumber
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string? ImplementationConventionReference
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }
}