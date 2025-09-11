namespace EdiSource._270_5010.Loop2000D_Dependent.Segments;

[SegmentGenerator<_270_5010_Loop2000D_Dependent>("TRN")]
public sealed partial class _270_5010_Loop2000D_TRN_TraceNumber
{
    public string? TraceTypeCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string? DependentTraceNumber
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