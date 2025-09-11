
namespace EdiSource._270_5010.Loop2000B_InformationReceiver.Loop1000B.Segments;

[SegmentGenerator<_270_5010_Loop1000B_InformationReceiverName>("NM1", $"1P{S}2B{S}36{S}80{S}FA{S}GP{S}P5{S}PR")]
public sealed partial class _270_5010_Loop1000B_NM1_InformationReceiverName
{
    public string? EntityIdentifierCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }
    
    public string? EntityTypeQualifier
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }
    
    public string? InformationReceiverName
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }
    
    public string? InformationReceiverFirstName
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }
    
    public string? InformationReceiverMiddleName
    {
        get => GetCompositeElement(5);
        set => SetCompositeElement(value, 5);
    }
    
    public string? NamePrefix
    {
        get => GetCompositeElement(6);
        set => SetCompositeElement(value, 6);
    }
    
    public string? NameSuffix
    {
        get => GetCompositeElement(7);
        set => SetCompositeElement(value, 7);
    }
    
    public string? IdentificationCodeQualifier
    {
        get => GetCompositeElement(8);
        set => SetCompositeElement(value, 8);
    }
    
    public string? InformationReceiverIdentifier
    {
        get => GetCompositeElement(9);
        set => SetCompositeElement(value, 9);
    }
}