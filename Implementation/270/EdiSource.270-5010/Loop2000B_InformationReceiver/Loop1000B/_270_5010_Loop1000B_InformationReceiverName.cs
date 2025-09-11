
using EdiSource._270_5010.Loop2000B_InformationReceiver.Loop1000B.Segments;

namespace EdiSource._270_5010.Loop2000B_InformationReceiver.Loop1000B;

[LoopGenerator<_270_5010_Loop2000B_InformationReceiver, _270_5010_Loop1000B_InformationReceiverName, _270_5010_Loop1000B_NM1_InformationReceiverName>]
public sealed partial class _270_5010_Loop1000B_InformationReceiverName : IValidatable
{
    [SegmentHeader] public _270_5010_Loop1000B_NM1_InformationReceiverName NM1_InformationReceiverName { get; set; } = null!;
    
    [SegmentList] public SegmentList<_270_5010_Loop1000B_REF_AdditionalID> REF_AdditionalIDs { get; set; } = [];
    
    [Segment] public _270_5010_Loop1000B_N3_Address? N3_Address { get; set; }
    
    [Segment] public _270_5010_Loop1000B_N4_CityStateZIP? N4_CityStateZIP { get; set; }
    
    [Segment] public _270_5010_Loop1000B_PRV_ProviderInfo? PRV_ProviderInfo { get; set; }
    
    public IEnumerable<ValidationMessage> Validate()
    {
        if (NM1_InformationReceiverName == null)
            yield return ValidationFactory.Create((ILoop)this, ValidationSeverity.Critical, 
                "NM1 Information Receiver Name segment is required");
                
        // Business rule: N4 should be present when N3 is present
        if (N3_Address != null && N4_CityStateZIP == null)
            yield return ValidationFactory.Create((ILoop)this, ValidationSeverity.Warning,
                "N4 City/State/ZIP segment is recommended when N3 Address segment is present");
                
        // Business validation: REF segment limit (max 9)
        if (REF_AdditionalIDs.Count > 9)
            yield return ValidationFactory.Create((ILoop)this, ValidationSeverity.Error,
                "Maximum of 9 REF Additional ID segments allowed");
    }
}