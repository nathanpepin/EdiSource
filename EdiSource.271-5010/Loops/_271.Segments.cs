namespace EdiSource._271_5010.Loops;

[SegmentGenerator<_271>("ST")]
public partial class TS271_ST
{
    public string TransactionSetIdentifierCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string TransactionSetControlNumber
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string ImplementationConventionReference
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }
}

[SegmentGenerator<_271>("BHT")]
public partial class TS271_BHT
{
    public string HierarchicalStructureCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string TransactionSetPurposeCode
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string SubmitterTransactionIdentifier
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string TransactionSetCreationDate
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }

    public string TransactionSetCreationTime
    {
        get => GetCompositeElement(5);
        set => SetCompositeElement(value, 5);
    }
}

[SegmentGenerator<_271>("SE")]
public partial class TS271_SE
{
    public string TransactionSegmentCount
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string TransactionSetControlNumber
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }
}