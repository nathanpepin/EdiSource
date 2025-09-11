namespace EdiSource._270_5010.TransactionSet.Loop2000C_Subscriber.Loop2100C.Segments;

[SegmentGenerator<_270_5010_Loop2100C_SubscriberName>("NM1", "IL")]
public sealed partial class _270_5010_Loop2100C_NM1_SubscriberName
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
    
    public string? SubscriberLastName
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }
    
    public string? SubscriberFirstName
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }
    
    public string? SubscriberMiddleName
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
    
    public string? SubscriberPrimaryIdentifier
    {
        get => GetCompositeElement(9);
        set => SetCompositeElement(value, 9);
    }
}