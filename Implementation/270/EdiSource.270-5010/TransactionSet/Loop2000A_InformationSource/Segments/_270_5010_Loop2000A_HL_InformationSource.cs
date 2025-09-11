namespace EdiSource._270_5010.TransactionSet.Loop2000A_InformationSource.Segments;

[SegmentGenerator<_270_5010_Loop2000A_InformationSource>("HL", null, null, "20")]
public sealed partial class _270_5010_Loop2000A_HL_InformationSource
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