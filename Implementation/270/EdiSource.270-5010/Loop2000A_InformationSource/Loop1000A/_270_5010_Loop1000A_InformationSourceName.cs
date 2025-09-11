using EdiSource._270_5010.Loop2000A_InformationSource.Loop1000A.Segments;

namespace EdiSource._270_5010.Loop2000A_InformationSource.Loop1000A;

[LoopGenerator<_270_5010_Loop2000A_InformationSource, _270_5010_Loop1000A_InformationSourceName, _270_5010_Loop1000A_NM1_InformationSourceName>]
public sealed partial class _270_5010_Loop1000A_InformationSourceName : IValidatable
{
    [SegmentHeader] public _270_5010_Loop1000A_NM1_InformationSourceName NM1_InformationSourceName { get; set; } = null!;
    
    public IEnumerable<ValidationMessage> Validate()
    {
        if (NM1_InformationSourceName == null)
            yield return ValidationFactory.Create((ILoop)this, ValidationSeverity.Critical, 
                "NM1 Information Source Name segment is required");
                
        // Business validation: Information source must be payer (PR)
        if (NM1_InformationSourceName?.EntityIdentifierCode != "PR")
            yield return ValidationFactory.Create((ILoop)this, ValidationSeverity.Error,
                "Information Source must have Entity Identifier Code 'PR' (Payer)");
    }
}