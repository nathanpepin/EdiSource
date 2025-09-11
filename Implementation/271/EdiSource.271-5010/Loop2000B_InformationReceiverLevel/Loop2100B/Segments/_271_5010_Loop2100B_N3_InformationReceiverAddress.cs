namespace EdiSource._271_5010.Loop2000B_InformationReceiverLevel.Loop2100B.Segments;

[SegmentGenerator<Loop2100B._271_5010_Loop2100B_InformationReceiverName>("N3")]
public sealed partial class _271_5010_Loop2100B_N3_InformationReceiverAddress
{
    public string? AddressLine1 { get => GetCompositeElement(1); set => SetCompositeElement(value, 1); }
    public string? AddressLine2 { get => GetCompositeElement(2); set => SetCompositeElement(value, 2); }
}
