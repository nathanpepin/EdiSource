namespace EdiSource._270_5010.Loop2000C_Subscriber.Loop2100C.Loop2110C.Segments;

[SegmentGenerator<_270_5010_Loop2110C_EligibilityBenefitInquiry>("REF")]
public sealed partial class _270_5010_Loop2110C_REF_AdditionalInfo
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
    
    public string? ReferenceIdentifier
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }
}