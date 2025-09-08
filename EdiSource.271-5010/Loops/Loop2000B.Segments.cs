namespace EdiSource._271_5010.Loops;

[SegmentGenerator<Loop2000B>("HL", null, null, "21")]
public partial class Loop2000B_HL
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