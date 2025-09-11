using EdiSource._271_5010.Loop2000A_InformationSourceLevel.Loop2100A.Segments;

namespace EdiSource._271_5010.Loop2000A_InformationSourceLevel.Loop2100A;

[LoopGenerator<_271_5010_Loop2000A_InformationSourceLevel, _271_5010_Loop2100A_InformationSourceName, _271_5010_Loop2100A_NM1_InformationSourceName>]
public sealed partial class _271_5010_Loop2100A_InformationSourceName : IValidatable
{
    [SegmentHeader] public _271_5010_Loop2100A_NM1_InformationSourceName NM1_InformationSourceName { get; set; } = null!;

    [Segment] public _271_5010_Loop2100A_N3_InformationSourceAddress? N3_InformationSourceAddress { get; set; }
    [Segment] public _271_5010_Loop2100A_N4_InformationSourceGeographicLocation? N4_InformationSourceGeographicLocation { get; set; }
    [Segment] public _271_5010_Loop2100A_PER_InformationSourceContactInfo? PER_InformationSourceContactInfo { get; set; }
    [Segment] public _271_5010_Loop2100A_PRV_InformationSourceProviderInfo? PRV_InformationSourceProviderInfo { get; set; }

    public IEnumerable<ValidationMessage> Validate()
    {
        if (NM1_InformationSourceName == null)
            yield return ValidationFactory.Create((ILoop)this, ValidationSeverity.Critical, "NM1 segment is required");
    }
}