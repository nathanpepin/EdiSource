using EdiSource._271_5010.Loop2000B_InformationReceiverLevel;
using EdiSource._271_5010.Loop2000C_SubscriberLevel.Loop2100C;
using EdiSource._271_5010.Loop2000C_SubscriberLevel.Segments;
using EdiSource._271_5010.Loop2000D_DependentLevel;

namespace EdiSource._271_5010.Loop2000C_SubscriberLevel;

[LoopGenerator<_271_5010_Loop2000B_InformationReceiverLevel, _271_5010_Loop2000C_SubscriberLevel, _271_5010_Loop2000C_HL_SubscriberLevel>]
public sealed partial class _271_5010_Loop2000C_SubscriberLevel : IValidatable
{
    [SegmentHeader] public _271_5010_Loop2000C_HL_SubscriberLevel HL_SubscriberLevel { get; set; } = null!;

    [Segment] public _271_5010_Loop2000C_TRN_SubscriberTraceNumber? TRN_SubscriberTraceNumber { get; set; }

    [LoopList] public LoopList<_271_5010_Loop2100C_SubscriberName> Loop2100C_SubscriberNames { get; set; } = [];
    [LoopList] public LoopList<_271_5010_Loop2000D_DependentLevel> Loop2000D_DependentLevels { get; set; } = [];

    public IEnumerable<ValidationMessage> Validate()
    {
        if (HL_SubscriberLevel == null)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "HL segment is required");
    }
}