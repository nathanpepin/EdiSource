using EdiSource._270_5010.Loop2000C_Subscriber;

namespace EdiSource._270_5010.TransactionSet.Loop2000C_Subscriber.Segments;

[SegmentGenerator<_270_5010_Loop2000C_Subscriber>("TRN")]
public sealed partial class _270_5010_Loop2000C_TRN_TraceNumber
{
    public string? TraceTypeCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string? TraceNumber
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string? TraceAssigningEntityIdentifier
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string? TraceAssigningEntityAdditionalIdentifier
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }
}