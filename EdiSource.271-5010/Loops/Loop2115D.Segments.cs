namespace EdiSource._271_5010.Loops;

[SegmentGenerator<Loop2115D>("III")]
public partial class Loop2115D_III
{
    public string CodeListQualifierCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string IndustryCode
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string CodeCategory
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string InjuredBodyPartName
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }
}