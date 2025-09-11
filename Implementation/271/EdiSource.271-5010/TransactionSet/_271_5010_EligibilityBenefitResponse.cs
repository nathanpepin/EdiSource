using EdiSource._271_5010.Loop1000A_InformationSourceName;
using EdiSource._271_5010.Loop1000B_InformationReceiverName;
using EdiSource._271_5010.Loop2000A_InformationSourceLevel;
using EdiSource._271_5010.TransactionSet.Segments;

namespace EdiSource._271_5010.TransactionSet;

[LoopGenerator<FunctionalGroup, _271_5010_EligibilityBenefitResponse, _271_5010_ST_TransactionSetHeader>]
public sealed partial class _271_5010_EligibilityBenefitResponse : IValidatable, ITransactionSet<_271_5010_EligibilityBenefitResponse>
{
    [SegmentHeader] public _271_5010_ST_TransactionSetHeader ST_TransactionSetHeader { get; set; } = null!;

    [Segment] public _271_5010_BHT_BeginningOfHierarchicalTransaction? BHT_BeginningOfHierarchicalTransaction { get; set; }

    [LoopList] public LoopList<_271_5010_Loop1000A_InformationSourceName> Loop1000A_InformationSourceNames { get; set; } = [];
    [LoopList] public LoopList<_271_5010_Loop1000B_InformationReceiverName> Loop1000B_InformationReceiverNames { get; set; } = [];
    [LoopList] public LoopList<_271_5010_Loop2000A_InformationSourceLevel> Loop2000A_InformationSourceLevels { get; set; } = [];

    [SegmentFooter] public _271_5010_SE_TransactionSetTrailer SE_TransactionSetTrailer { get; set; } = null!;

    public static TransactionSetDefinition Definition { get; } =
        TransactionSetDefinitionsFactory<_271_5010_EligibilityBenefitResponse>.CreateDefinition();

    public IEnumerable<ValidationMessage> Validate()
    {
        if (ST_TransactionSetHeader == null)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "ST segment is required");
        if (SE_TransactionSetTrailer == null)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "SE segment is required");
        if (BHT_BeginningOfHierarchicalTransaction == null)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "BHT segment is required");
    }
}