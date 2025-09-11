using EdiSource._270_5010.Loop2000C_Subscriber.Loop2100C;
using EdiSource._270_5010.TransactionSet.Loop2000C_Subscriber.Segments;

namespace EdiSource._270_5010.Loop2000C_Subscriber;

[LoopGenerator<_270_5010_EligibilityBenefitInquiry, _270_5010_Loop2000C_Subscriber, _270_5010_Loop2000C_HL_Subscriber>]
public sealed partial class _270_5010_Loop2000C_Subscriber : IValidatable
{
    [SegmentHeader] public _270_5010_Loop2000C_HL_Subscriber HL_Subscriber { get; set; } = null!;

    [SegmentList] public SegmentList<_270_5010_Loop2000C_TRN_TraceNumber> TRN_TraceNumbers { get; set; } = [];

    [Loop] public _270_5010_Loop2100C_SubscriberName? Loop2100C_SubscriberName { get; set; }

    // Note: Additional loops will be added in subsequent phases
    // [LoopList] public LoopList<_270_5010_Loop2110C_EligibilityBenefitInquiry> Loop2110C_EligibilityBenefitInquiries { get; set; } = [];
    // [Loop] public _270_5010_Loop2000D_Dependent? Loop2000D_Dependent { get; set; }

    public IEnumerable<ValidationMessage> Validate()
    {
        if (HL_Subscriber == null)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical,
                "HL Subscriber segment is required");

        // Business validation: Must have subscriber name loop
        if (Loop2100C_SubscriberName == null)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical,
                "Subscriber Name loop (2100C) is required");

        // Business validation: TRN segment limit
        if (TRN_TraceNumbers.Count > 2)
            yield return ValidationFactory.Create(this, ValidationSeverity.Error,
                "Maximum of 2 TRN Trace Number segments allowed");
    }
}