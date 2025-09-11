namespace EdiSource._270_5010.Loop2000C_Subscriber.Loop2100C.Segments;

[SegmentGenerator<_270_5010_Loop2100C_SubscriberName>("REF")]
public sealed partial class _270_5010_Loop2100C_REF_AdditionalID
{
    public string? ReferenceIdentificationQualifier
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string? ReferenceIdentification
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string? Description
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }
}