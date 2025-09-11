using EdiSource._271_5010.Loop2000D_DependentLevel.Loop2100D_DependentName;

namespace EdiSource._271_5010.Loop2000D_DependentLevel.Loop2100D.Segments;

[SegmentGenerator<_271_5010_Loop2100D_DependentName>("REF", "6P")]
public sealed partial class _271_5010_Loop2100D_REF_GroupNumber
{
    public string? ReferenceIdentificationQualifier { get => GetCompositeElement(1); set => SetCompositeElement(value, 1); }
    public string? GroupNumber { get => GetCompositeElement(2); set => SetCompositeElement(value, 2); }
    public string? Description { get => GetCompositeElement(3); set => SetCompositeElement(value, 3); }
    public string? ReferenceIdentifier { get => GetCompositeElement(4); set => SetCompositeElement(value, 4); }
}
