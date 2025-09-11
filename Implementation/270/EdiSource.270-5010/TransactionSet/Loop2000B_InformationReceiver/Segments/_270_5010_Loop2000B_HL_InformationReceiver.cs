namespace EdiSource._270_5010.TransactionSet.Loop2000B_InformationReceiver.Segments;

[SegmentGenerator<_270_5010_Loop2000B_InformationReceiver>("HL", null, null, "21")]
public sealed partial class _270_5010_Loop2000B_HL_InformationReceiver
{
    public int? HierarchicalIdNumber
    {
        get => this.GetInt(1);
        set => this.SetInt(value, 1);
    }
    
    public int? HierarchicalParentIdNumber
    {
        get => this.GetInt(2);
        set => this.SetInt(value, 2);
    }
    
    public string? HierarchicalLevelCode
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }
    
    public bool? HierarchicalChildCode
    {
        get => this.GetBool(4, "1", falseValue: "0");
        set => this.SetBool(value, "1", "0", 4);
    }
}