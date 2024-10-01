using EdiSource._270_5010.Loop2000A_InformationSource;
using EdiSource._270_5010.Loop2000B_InformationReceiver;
using EdiSource._270_5010.Loop2000C_Subscriber;
using EdiSource._270_5010.Loop2000D_Dependent;

namespace EdiSource._270_5010.TransactionSet;

[LoopGenerator<FunctionalGroup, _270_5010_EligibilityBenefitInquiry, _270_5010_ST_TransactionSetHeader>]
public sealed partial class _270_5010_EligibilityBenefitInquiry : IValidatable, ITransactionSet<_270_5010_EligibilityBenefitInquiry>
{
    [SegmentHeader] public _270_5010_ST_TransactionSetHeader ST_TransactionSetHeader { get; set; } = null!;

    [Segment] public _270_5010_BHT_BeginningHierarchicalTransaction? BHT_BeginningHierarchicalTransaction { get; set; }

    // Information Source (payers, insurance companies)
    [LoopList] public LoopList<_270_5010_Loop2000A_InformationSource> Loop2000A_InformationSources { get; set; } = [];

    // Information Receiver (providers, clearinghouses) 
    [LoopList] public LoopList<_270_5010_Loop2000B_InformationReceiver> Loop2000B_InformationReceivers { get; set; } = [];

    // Subscriber levels (patients/members)
    [LoopList] public LoopList<_270_5010_Loop2000C_Subscriber> Loop2000C_Subscribers { get; set; } = [];

    // Subscriber levels (dependents)
    [LoopList] public LoopList<_270_5010_Loop2000D_Dependent> Loop2000D_Dependents { get; set; } = [];

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

        // Business validation: At least one information source required
        if (!Loop2000A_InformationSources.Any())
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical,
                "At least one information source (Loop 2000A) is required");

        // Business validation: At least one information receiver required
        if (!Loop2000B_InformationReceivers.Any())
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical,
                "At least one information receiver (Loop 2000B) is required");

        // Business validation: At least one subscriber level required
        if (!Loop2000C_Subscribers.Any())
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical,
                "At least one subscriber level (Loop 2000C) is required");
    }
}