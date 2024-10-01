namespace EdiSource._271_5010.TransactionSet.Segments;

[SegmentGenerator<_271_5010_EligibilityBenefitResponse>("BHT")]
public sealed partial class _271_5010_BHT_BeginningOfHierarchicalTransaction
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

    public DateTime? TransactionSetCreationDate
    {
        get => this.GetDate(4);
        set => this.SetDate(value, 4);
    }

    public TimeOnly? TransactionSetCreationTime
    {
        get => this.GetTimeOnly(5);
        set => this.SetTimeOnly(value, 5);
    }
}