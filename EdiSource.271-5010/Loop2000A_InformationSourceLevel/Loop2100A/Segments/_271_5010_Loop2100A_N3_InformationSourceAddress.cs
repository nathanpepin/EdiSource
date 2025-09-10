namespace EdiSource._271_5010.Loop2000A_InformationSourceLevel.Loop2100A_InformationSourceName.Segments;

[SegmentGenerator<_271_5010_Loop2100A_InformationSourceName>("N3")]
public partial class _271_5010_Loop2100A_N3_InformationSourceAddress
{
    public string? AddressLine1 { get => GetCompositeElement(1); set => SetCompositeElement(value, 1); }
    public string? AddressLine2 { get => GetCompositeElement(2); set => SetCompositeElement(value, 2); }
}
