namespace EdiSource._270_5010.Loop2000C_Subscriber.Loop2100C.Loop2110C.Segments;

[SegmentGenerator<_270_5010_Loop2110C_EligibilityBenefitInquiry>("III")]
public sealed partial class _270_5010_Loop2110C_III_AdditionalInquiry
{
    public string? QualifierCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string? IndustryCode
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }
}