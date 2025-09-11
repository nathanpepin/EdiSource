using EdiSource._271_5010.Loop2000C_SubscriberLevel.Loop2100C.Loop2110C.Loop2120C.Loop2120C.Segments;

namespace EdiSource._271_5010.Loop2000C_SubscriberLevel.Loop2100C.Loop2110C.Loop2120C.Loop2120C;

[LoopGenerator<_271_5010_Loop2120C_BenefitRelatedEntityGrouping, _271_5010_Loop2120C_BenefitRelatedEntity, _271_5010_Loop2120C_NM1_BenefitRelatedEntityName>]
public sealed partial class _271_5010_Loop2120C_BenefitRelatedEntity : IValidatable
{
    [SegmentHeader] public _271_5010_Loop2120C_NM1_BenefitRelatedEntityName NM1_BenefitRelatedEntityName { get; set; } = null!;

    [Segment] public _271_5010_Loop2120C_N3_BenefitEntityAddress? N3_BenefitRelatedEntityAddress { get; set; }
    [Segment] public _271_5010_Loop2120C_N4_BenefitRelatedLocation? N4_BenefitRelatedEntityGeographicLocation { get; set; }
    [Segment] public _271_5010_Loop2120C_PER_BenefitRelatedEntityContactInfo? PER_BenefitRelatedEntityContactInfo { get; set; }
    [Segment] public _271_5010_Loop2120C_PRV_BenefitRelatedEntityProviderInfo? PRV_BenefitRelatedEntityProviderInfo { get; set; }

    public IEnumerable<ValidationMessage> Validate()
    {
        if (NM1_BenefitRelatedEntityName == null)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "NM1 segment is required");
    }
}