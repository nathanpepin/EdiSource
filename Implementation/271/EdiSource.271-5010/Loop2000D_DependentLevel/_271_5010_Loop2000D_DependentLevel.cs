using EdiSource._271_5010.Loop2000C_SubscriberLevel;
using EdiSource._271_5010.Loop2000D_DependentLevel.Loop2100D;
using EdiSource._271_5010.Loop2000D_DependentLevel.Segments;

namespace EdiSource._271_5010.Loop2000D_DependentLevel;

[LoopGenerator<_271_5010_Loop2000C_SubscriberLevel, _271_5010_Loop2000D_DependentLevel, _271_5010_Loop2000D_HL_DependentLevel>]
public sealed partial class _271_5010_Loop2000D_DependentLevel : IValidatable
{
    [SegmentHeader] public _271_5010_Loop2000D_HL_DependentLevel HL_DependentLevel { get; set; } = null!;

    [Segment] public _271_5010_Loop2000D_TRN_DependentTraceNumber? TRN_DependentTraceNumber { get; set; }

    [LoopList] public LoopList<_271_5010_Loop2100D_DependentName> Loop2100D_DependentNames { get; set; } = [];

    public IEnumerable<ValidationMessage> Validate()
    {
        if (HL_DependentLevel == null)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "HL segment is required");
    }
}