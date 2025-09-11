using EdiSource._271_5010.Loop2000B_InformationReceiverLevel.Loop2100B.Segments;

namespace EdiSource._271_5010.Loop2000B_InformationReceiverLevel.Loop2100B;

[LoopGenerator<_271_5010_Loop2000B_InformationReceiverLevel, _271_5010_Loop2100B_InformationReceiverName, _271_5010_Loop2100B_NM1_InformationReceiverName>]
public sealed partial class _271_5010_Loop2100B_InformationReceiverName : IValidatable
{
    [SegmentHeader] public _271_5010_Loop2100B_NM1_InformationReceiverName NM1_InformationReceiverName { get; set; } = null!;

    [Segment] public _271_5010_Loop2100B_N3_InformationReceiverAddress? N3_InformationReceiverAddress { get; set; }
    [Segment] public _271_5010_Loop2100B_N4_InformationReceiverGeographicLocation? N4_InformationReceiverGeographicLocation { get; set; }
    [Segment] public _271_5010_Loop2100B_PER_InformationReceiverContactInfo? PER_InformationReceiverContactInfo { get; set; }
    [Segment] public _271_5010_Loop2100B_PRV_InformationReceiverProviderInfo? PRV_InformationReceiverProviderInfo { get; set; }

    public IEnumerable<ValidationMessage> Validate()
    {
        if (NM1_InformationReceiverName == null)
            yield return ValidationFactory.Create((ILoop)this, ValidationSeverity.Critical, "NM1 segment is required");
    }
}
