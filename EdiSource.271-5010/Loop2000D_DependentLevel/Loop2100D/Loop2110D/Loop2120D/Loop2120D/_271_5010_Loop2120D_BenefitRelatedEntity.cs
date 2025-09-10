using EdiSource._271_5010.Loop2000D_DependentLevel.Loop2100D_DependentName.Loop2110D_DependentEligibilityOrBenefitInfo.Loop2120D_BenefitRelatedEntityGrouping.Loop2120D_BenefitRelatedEntity.Segments;

namespace EdiSource._271_5010.Loop2000D_DependentLevel.Loop2100D_DependentName.Loop2110D_DependentEligibilityOrBenefitInfo.Loop2120D_BenefitRelatedEntityGrouping.Loop2120D_BenefitRelatedEntity;

[LoopGenerator<_271_5010_Loop2120D_BenefitRelatedEntityGrouping, _271_5010_Loop2120D_BenefitRelatedEntity, _271_5010_Loop2120D_NM1_BenefitRelatedEntityName>]
public partial class _271_5010_Loop2120D_BenefitRelatedEntity : IValidatable
{
    [SegmentHeader] public _271_5010_Loop2120D_NM1_BenefitRelatedEntityName NM1_BenefitRelatedEntityName { get; set; } = null!;

   [Segment] public _271_5010_Loop2120D_N3_BenefitRelatedEntityAddress? N3_BenefitRelatedEntityAddress { get; set; }
   [Segment] public _271_5010_Loop2120D_N4_BenefitRelatedEntityGeographicLocation? N4_BenefitRelatedEntityGeographicLocation { get; set; }
   [Segment] public _271_5010_Loop2120D_PER_BenefitRelatedEntityContactInfo? PER_BenefitRelatedEntityContactInfo { get; set; }
   [Segment] public _271_5010_Loop2120D_PRV_BenefitEntityProviderInfo? PRV_BenefitRelatedEntityProviderInfo { get; set; }

    public IEnumerable<ValidationMessage> Validate()
    {
        if (NM1_BenefitRelatedEntityName == null)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "NM1 segment is required");
    }
}
