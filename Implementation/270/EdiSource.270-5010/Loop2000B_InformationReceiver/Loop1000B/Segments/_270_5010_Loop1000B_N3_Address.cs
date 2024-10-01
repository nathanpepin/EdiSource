namespace EdiSource._270_5010.Loop2000B_InformationReceiver.Loop1000B.Segments;

[SegmentGenerator<_270_5010_Loop1000B_InformationReceiverName>("N3")]
public sealed partial class _270_5010_Loop1000B_N3_Address
{
    public string? AddressInformation
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string? AdditionalAddressInformation
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }
}