
using EdiSource._270_5010.Loop2000A_InformationSource.Loop1000A;
using EdiSource._270_5010.Loop2000A_InformationSource.Segments;

namespace EdiSource._270_5010.Loop2000A_InformationSource;

[LoopGenerator<_270_5010_EligibilityBenefitInquiry, _270_5010_Loop2000A_InformationSource, _270_5010_Loop2000A_HL_InformationSource>]
public sealed partial class _270_5010_Loop2000A_InformationSource : IValidatable
{
    [SegmentHeader] public _270_5010_Loop2000A_HL_InformationSource HL_InformationSource { get; set; } = null!;
    
    [Loop] public _270_5010_Loop1000A_InformationSourceName? Loop1000A_InformationSourceName { get; set; }
    
    public IEnumerable<ValidationMessage> Validate()
    {
        if (HL_InformationSource == null)
            yield return ValidationFactory.Create((ILoop)this, ValidationSeverity.Critical, 
                "HL Information Source segment is required");
                
        // Business validation: Must have Information Source Name loop
        if (Loop1000A_InformationSourceName == null)
            yield return ValidationFactory.Create((ILoop)this, ValidationSeverity.Critical,
                "Information Source Name loop (1000A) is required");
    }
}