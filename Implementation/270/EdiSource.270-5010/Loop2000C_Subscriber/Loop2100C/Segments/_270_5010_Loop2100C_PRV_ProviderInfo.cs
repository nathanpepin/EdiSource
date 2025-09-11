namespace EdiSource._270_5010.Loop2000C_Subscriber.Loop2100C.Segments;

[SegmentGenerator<_270_5010_Loop2100C_SubscriberName>("PRV")]
public sealed partial class _270_5010_Loop2100C_PRV_ProviderInfo
{
    public string? ProviderCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }
    
    public string? ReferenceIdentificationQualifier
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }
    
    public string? ProviderTaxonomyCode
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }
}