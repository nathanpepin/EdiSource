namespace EdiSource._270_5010.TransactionSet.Segments;

[SegmentGenerator<_270_5010_EligibilityBenefitInquiry>("BHT")]
public sealed partial class _270_5010_BHT_BeginningHierarchicalTransaction
{
    public string? HierarchicalStructureCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string? TransactionSetPurposeCode
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string? SubmitterTransactionIdentifier
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public DateOnly? TransactionSetCreationDate
    {
        get => this.GetDateOnly(4);
        set => this.SetDateOnly(value, 4);
    }

    public TimeOnly? TransactionSetCreationTime
    {
        get => this.GetTimeOnly(5);
        set => this.SetTimeOnly(value, 5);
    }
}