namespace EdiSource._271_5010.Loop2000C_SubscriberLevel.Loop2100C_SubscriberName.Segments;

[SegmentGenerator<_271_5010_Loop2100C_SubscriberName>("REF", "SY")]
public partial class _271_5010_Loop2100C_REF_SocialSecurityNumber
{
    public string? ReferenceIdentificationQualifier { get => GetCompositeElement(1); set => SetCompositeElement(value, 1); }
    public string? SocialSecurityNumber { get => GetCompositeElement(2); set => SetCompositeElement(value, 2); }
    public string? Description { get => GetCompositeElement(3); set => SetCompositeElement(value, 3); }
    public string? ReferenceIdentifier { get => GetCompositeElement(4); set => SetCompositeElement(value, 4); }
}
