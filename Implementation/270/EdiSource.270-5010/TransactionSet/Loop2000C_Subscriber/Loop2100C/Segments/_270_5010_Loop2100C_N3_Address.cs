namespace EdiSource._270_5010.TransactionSet.Loop2000C_Subscriber.Loop2100C.Segments;

[SegmentGenerator<_270_5010_Loop2100C_SubscriberName>("N3")]
public sealed partial class _270_5010_Loop2100C_N3_Address
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