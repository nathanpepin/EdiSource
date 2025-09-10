namespace EdiSource._271_5010.Loop2000C_SubscriberLevel.Segments;

[SegmentGenerator<_271_5010_Loop2000C_SubscriberLevel>("TRN")]
public partial class _271_5010_Loop2000C_TRN_SubscriberTraceNumber
{
    public string? TraceTypeCode { get => GetCompositeElement(1); set => SetCompositeElement(value, 1); }
    public string? SubscriberTraceNumber { get => GetCompositeElement(2); set => SetCompositeElement(value, 2); }
    public string? TraceAssigningEntityIdentifier { get => GetCompositeElement(3); set => SetCompositeElement(value, 3); }
    public string? TraceAssigningEntityAdditionalIdentifier { get => GetCompositeElement(4); set => SetCompositeElement(value, 4); }
}
