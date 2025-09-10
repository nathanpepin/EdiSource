namespace EdiSource._271_5010.Loop2000B_InformationReceiverLevel.Loop2100B_InformationReceiverName.Segments;

[SegmentGenerator<_271_5010_Loop2100B_InformationReceiverName>("PRV")]
public partial class _271_5010_Loop2100B_PRV_InformationReceiverProviderInfo
{
    public string? ProviderCode { get => GetCompositeElement(1); set => SetCompositeElement(value, 1); }
    public string? ReferenceIdentificationQualifier { get => GetCompositeElement(2); set => SetCompositeElement(value, 2); }
    public string? InformationReceiverProviderTaxonomyCode { get => GetCompositeElement(3); set => SetCompositeElement(value, 3); }
}
