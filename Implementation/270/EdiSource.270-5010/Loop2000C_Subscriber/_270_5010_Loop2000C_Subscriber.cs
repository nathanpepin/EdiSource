using EdiSource._270_5010.Loop2000C_Subscriber.Loop2100C;
using EdiSource._270_5010.Loop2000C_Subscriber.Loop2100C.Loop2110C;
using EdiSource._270_5010.Loop2000C_Subscriber.Segments;

namespace EdiSource._270_5010.Loop2000C_Subscriber;

[LoopGenerator<_270_5010_EligibilityBenefitInquiry, _270_5010_Loop2000C_Subscriber, _270_5010_Loop2000C_HL_Subscriber>]
public sealed partial class _270_5010_Loop2000C_Subscriber : IValidatable
{
    [SegmentHeader] public _270_5010_Loop2000C_HL_Subscriber HL_Subscriber { get; set; } = null!;
    
    [SegmentList] public SegmentList<_270_5010_Loop2000C_TRN_TraceNumber> TRN_TraceNumbers { get; set; } = [];
    
    [Loop] public _270_5010_Loop2100C_SubscriberName? Loop2100C_SubscriberName { get; set; }
    
    // Eligibility/Benefit Inquiry loops
    [LoopList] public LoopList<_270_5010_Loop2110C_EligibilityBenefitInquiry> Loop2110C_EligibilityBenefitInquiries { get; set; } = [];
    
    // Note: Dependent loop will be added in subsequent phase
    // [Loop] public _270_5010_Loop2000D_Dependent? Loop2000D_Dependent { get; set; }
    
    public IEnumerable<ValidationMessage> Validate()
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference
        if (HL_Subscriber == null)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, 
                "HL Subscriber segment is required");
#pragma warning restore CS8602
                
        // Business validation: Must have subscriber name loop
        if (Loop2100C_SubscriberName == null)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical,
                "Subscriber Name loop (2100C) is required");
                
        // Business validation: Must have at least one eligibility inquiry
        if (!Loop2110C_EligibilityBenefitInquiries.Any())
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical,
                "At least one Eligibility/Benefit Inquiry loop (2110C) is required");
                
        // Business validation: TRN segment limit
        if (TRN_TraceNumbers.Count > 2)
            yield return ValidationFactory.Create(this, ValidationSeverity.Error,
                "Maximum of 2 TRN Trace Number segments allowed");
    }
}