namespace EdiSource._271_5010.Loop2000D_DependentLevel.Segments;

[SegmentGenerator<_271_5010_Loop2000D_DependentLevel>("HL", null, null, "23")]
public partial class _271_5010_Loop2000D_HL_DependentLevel
{
    public int? HierarchicalIdNumber { get => this.GetInt(1); set => this.SetInt(value, 1); }
    public int? HierarchicalParentIdNumber { get => this.GetInt(2); set => this.SetInt(value, 2); }
    public string? HierarchicalLevelCode { get => GetCompositeElement(3); set => SetCompositeElement(value, 3); }
    public bool? HierarchicalChildCode { get => this.GetBool(4, "1", falseValue: "0"); set => this.SetBool(value, "1", "0", 4); }
}
