using EdiSource._270_5010.TransactionSet.Loop2000A_InformationSource;

namespace EdiSource._270_5010.TransactionSet;

[LoopGenerator<FunctionalGroup, _270_5010_EligibilityBenefitInquiry, _270_5010_ST_TransactionSetHeader>]
public sealed partial class _270_5010_EligibilityBenefitInquiry : IValidatable, ITransactionSet<_270_5010_EligibilityBenefitInquiry>
{
    [SegmentHeader] public _270_5010_ST_TransactionSetHeader ST_TransactionSetHeader { get; set; } = null!;
    
    [Segment] public _270_5010_BHT_BeginningHierarchicalTransaction? BHT_BeginningHierarchicalTransaction { get; set; }
    
    [LoopList] public LoopList<_270_5010_Loop2000A_InformationSource> Loop2000A_InformationSources { get; set; } = [];
    // [LoopList] public LoopList<_270_5010_Loop2000B_InformationReceiver> Loop2000B_InformationReceivers { get; set; } = [];
    // [LoopList] public LoopList<_270_5010_Loop2000C_Subscriber> Loop2000C_Subscribers { get; set; } = [];
    
    [SegmentFooter] public _270_5010_SE_TransactionSetTrailer SE_TransactionSetTrailer { get; set; } = null!;

    public static TransactionSetDefinition Definition { get; } =
        TransactionSetDefinitionsFactory<_270_5010_EligibilityBenefitInquiry>.CreateDefinition();

    public IEnumerable<ValidationMessage> Validate()
    {
        // Critical validation: ST and SE must exist
        if (ST_TransactionSetHeader == null)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "ST segment is required");
            
        if (SE_TransactionSetTrailer == null)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "SE segment is required");
        
        // Business validation: BHT is required for transaction purpose
        if (BHT_BeginningHierarchicalTransaction == null)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "BHT segment is required");
    }
}