
using EdiSource._270_5010.Loop2000C_Subscriber.Loop2100C.Segments;

namespace EdiSource._270_5010.Loop2000C_Subscriber.Loop2100C;

[LoopGenerator<_270_5010.Loop2000C_Subscriber._270_5010_Loop2000C_Subscriber, _270_5010_Loop2100C_SubscriberName, _270_5010_Loop2100C_NM1_SubscriberName>]
public sealed partial class _270_5010_Loop2100C_SubscriberName : IValidatable
{
    [SegmentHeader] public _270_5010_Loop2100C_NM1_SubscriberName NM1_SubscriberName { get; set; } = null!;
    
    [SegmentList] public SegmentList<_270_5010_Loop2100C_REF_AdditionalID> REF_AdditionalIDs { get; set; } = [];
    
    [Segment] public _270_5010_Loop2100C_N3_Address? N3_Address { get; set; }
    
    [Segment] public _270_5010_Loop2100C_N4_CityStateZIP? N4_CityStateZIP { get; set; }
    
    [Segment] public _270_5010_Loop2100C_PRV_ProviderInfo? PRV_ProviderInfo { get; set; }
    
    [Segment] public _270_5010_Loop2100C_DMG_Demographics? DMG_Demographics { get; set; }
    
    [Segment] public _270_5010_Loop2100C_INS_MultipleBirthSequence? INS_MultipleBirthSequence { get; set; }
    
    [Segment] public _270_5010_Loop2100C_HI_DiagnosisCode? HI_DiagnosisCode { get; set; }
    
    [SegmentList] public SegmentList<_270_5010_Loop2100C_DTP_Date> DTP_Dates { get; set; } = [];
    
    public IEnumerable<ValidationMessage> Validate()
    {
        if (NM1_SubscriberName == null)
            yield return ValidationFactory.Create((ILoop)this, ValidationSeverity.Critical, 
                "NM1 Subscriber Name segment is required");
                
        // Business validation: Subscriber must have entity identifier IL
        if (NM1_SubscriberName?.EntityIdentifierCode != "IL")
            yield return ValidationFactory.Create((ILoop)this, ValidationSeverity.Error,
                "Subscriber must have Entity Identifier Code 'IL' (Insured/Subscriber)");
                
        // Business rule: N4 should be present when N3 is present
        if (N3_Address != null && N4_CityStateZIP == null)
            yield return ValidationFactory.Create((ILoop)this, ValidationSeverity.Warning,
                "N4 City/State/ZIP segment is recommended when N3 Address segment is present");
                
        // Business validation: REF segment limit (max 9)
        if (REF_AdditionalIDs.Count > 9)
            yield return ValidationFactory.Create((ILoop)this, ValidationSeverity.Error,
                "Maximum of 9 REF Additional ID segments allowed");
                
        // Business validation: DTP segment limit (max 2)
        if (DTP_Dates.Count > 2)
            yield return ValidationFactory.Create((ILoop)this, ValidationSeverity.Error,
                "Maximum of 2 DTP Date segments allowed");
    }
}