namespace EdiSource._271_5010.Loop2000D_DependentLevel.Segments;

[SegmentGenerator<_271_5010_Loop2000D_DependentLevel>("TRN")]
public sealed partial class _271_5010_Loop2000D_TRN_DependentTraceNumber
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