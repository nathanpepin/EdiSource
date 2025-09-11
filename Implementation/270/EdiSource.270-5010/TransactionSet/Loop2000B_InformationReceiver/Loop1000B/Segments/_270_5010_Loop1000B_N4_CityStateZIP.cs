namespace EdiSource._270_5010.TransactionSet.Loop2000B_InformationReceiver.Loop1000B.Segments;

[SegmentGenerator<_270_5010_Loop1000B_InformationReceiverName>("N4")]
public sealed partial class _270_5010_Loop1000B_N4_CityStateZIP
{
    public string? CityName
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }
    
    public string? StateOrProvinceCode
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }
    
    public string? PostalCode
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }
    
    public string? CountryCode
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }
}