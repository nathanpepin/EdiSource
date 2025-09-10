using EdiSource._271_5010.Loop2000A_InformationSourceLevel;
using EdiSource._271_5010.Loop2000B_InformationReceiverLevel.Loop2100B_InformationReceiverName;
using EdiSource._271_5010.Loop2000B_InformationReceiverLevel.Segments;
using EdiSource._271_5010.Loop2000C_SubscriberLevel;

namespace EdiSource._271_5010.Loop2000B_InformationReceiverLevel;

[LoopGenerator<_271_5010_Loop2000A_InformationSourceLevel, _271_5010_Loop2000B_InformationReceiverLevel, _271_5010_Loop2000B_HL_InformationReceiverLevel>]
public partial class _271_5010_Loop2000B_InformationReceiverLevel : IValidatable
{
    [SegmentHeader] public _271_5010_Loop2000B_HL_InformationReceiverLevel HL_InformationReceiverLevel { get; set; } = null!;

    [LoopList] public LoopList<_271_5010_Loop2100B_InformationReceiverName> Loop2100B_InformationReceiverNames { get; set; } = [];
    [LoopList] public LoopList<_271_5010_Loop2000C_SubscriberLevel> Loop2000C_SubscriberLevels { get; set; } = [];

    public IEnumerable<ValidationMessage> Validate()
    {
        if (HL_InformationReceiverLevel == null)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "HL segment is required");
    }
}
