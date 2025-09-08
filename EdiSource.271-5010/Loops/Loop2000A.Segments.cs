using EdiSource.Domain.Identifiers;

namespace EdiSource._271_5010.Loops;

[SegmentGenerator<Loop2000A>("HL", null, null, "20")]
public partial class Loop2000A_HL
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

[SegmentGenerator<Loop2000A>("AAA")]
public partial class Loop2000A_AAA
{
    public string ValidRequestIndicator
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string RejectReasonCode
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string FollowUpActionCode
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }
}