using EdiSource._271_5010.Loop2000A_InformationSourceLevel.Loop2100A;
using EdiSource._271_5010.Loop2000A_InformationSourceLevel.Segments;
using EdiSource._271_5010.Loop2000B_InformationReceiverLevel;
using EdiSource._271_5010.TransactionSet;

namespace EdiSource._271_5010.Loop2000A_InformationSourceLevel;

[LoopGenerator<_271_5010_EligibilityBenefitResponse, _271_5010_Loop2000A_InformationSourceLevel, _271_5010_Loop2000A_HL_InformationSourceLevel>]
public sealed partial class _271_5010_Loop2000A_InformationSourceLevel : IValidatable
{
    [SegmentHeader] public _271_5010_Loop2000A_HL_InformationSourceLevel HL_InformationSourceLevel { get; set; } = null!;

    [LoopList] public LoopList<_271_5010_Loop2100A_InformationSourceName> Loop2100A_InformationSourceNames { get; set; } = [];
    [LoopList] public LoopList<_271_5010_Loop2000B_InformationReceiverLevel> Loop2000B_InformationReceiverLevels { get; set; } = [];

    public IEnumerable<ValidationMessage> Validate()
    {
        if (HL_InformationSourceLevel == null)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "HL segment is required");
    }
}