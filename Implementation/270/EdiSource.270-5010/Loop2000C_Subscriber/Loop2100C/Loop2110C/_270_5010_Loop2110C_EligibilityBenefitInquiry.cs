using EdiSource._270_5010.Loop2000C_Subscriber.Loop2100C.Loop2110C.Segments;

namespace EdiSource._270_5010.Loop2000C_Subscriber.Loop2100C.Loop2110C;

[LoopGenerator<_270_5010_Loop2000C_Subscriber, _270_5010_Loop2110C_EligibilityBenefitInquiry, _270_5010_Loop2110C_EQ_EligibilityBenefitInquiry>]
public sealed partial class _270_5010_Loop2110C_EligibilityBenefitInquiry : IValidatable
{
    [SegmentHeader] public _270_5010_Loop2110C_EQ_EligibilityBenefitInquiry EQ_EligibilityBenefitInquiry { get; set; } = null!;

    [Segment] public _270_5010_Loop2110C_AMT_SpendDownAmount? AMT_SpendDownAmount { get; set; }

    [Segment] public _270_5010_Loop2110C_AMT_SpendDownTotalBilled? AMT_SpendDownTotalBilled { get; set; }

    [Segment] public _270_5010_Loop2110C_III_AdditionalInquiry? III_AdditionalInquiry { get; set; }

    [Segment] public _270_5010_Loop2110C_REF_AdditionalInfo? REF_AdditionalInfo { get; set; }

    [Segment] public _270_5010_Loop2110C_DTP_EligibilityBenefitDate? DTP_EligibilityBenefitDate { get; set; }

    public IEnumerable<ValidationMessage> Validate()
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference
        if (EQ_EligibilityBenefitInquiry == null)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical,
                "EQ Eligibility/Benefit Inquiry segment is required");
#pragma warning restore CS8602
    }
}