using EdiSource._270_5010.Loop2000B_InformationReceiver.Loop1000B;
using EdiSource._270_5010.Loop2000B_InformationReceiver.Segments;

namespace EdiSource._270_5010.Loop2000B_InformationReceiver;

[LoopGenerator<_270_5010_EligibilityBenefitInquiry, _270_5010_Loop2000B_InformationReceiver, _270_5010_Loop2000B_HL_InformationReceiver>]
public sealed partial class _270_5010_Loop2000B_InformationReceiver : IValidatable
{
    [SegmentHeader] public _270_5010_Loop2000B_HL_InformationReceiver HL_InformationReceiver { get; set; } = null!;

    [Loop] public _270_5010_Loop1000B_InformationReceiverName? Loop1000B_InformationReceiverName { get; set; }

    public IEnumerable<ValidationMessage> Validate()
    {
        if (HL_InformationReceiver == null)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical,
                "HL Information Receiver segment is required");

        // Business validation: Must have Information Receiver Name loop
        if (Loop1000B_InformationReceiverName == null)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical,
                "Information Receiver Name loop (1000B) is required");
    }
}