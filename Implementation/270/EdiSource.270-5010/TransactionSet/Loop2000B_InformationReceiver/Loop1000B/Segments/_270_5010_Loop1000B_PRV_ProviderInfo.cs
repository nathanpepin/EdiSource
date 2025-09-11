namespace EdiSource._270_5010.TransactionSet.Loop2000B_InformationReceiver.Loop1000B.Segments;

[SegmentGenerator<_270_5010_Loop1000B_InformationReceiverName>("PRV")]
public sealed partial class _270_5010_Loop1000B_PRV_ProviderInfo
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