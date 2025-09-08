namespace EdiSource._271_5010.Loops;

[SegmentGenerator<Loop2000D>("HL", null, null, "23")]
public partial class Loop2000D_HL
{
    public string HierarchicalIdNumber
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string HierarchicalParentIdNumber
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string HierarchicalLevelCode
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string HierarchicalChildCode
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }
}

[SegmentGenerator<Loop2000D>("TRN")]
public partial class Loop2000D_TRN
{
    public string TraceTypeCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string TraceNumber
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string TraceAssigningEntityIdentifier
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string TraceAssigningEntityAdditionalIdentifier
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }
}