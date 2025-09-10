using EdiSource._271_5010.Loop2000C_SubscriberLevel.Loop2100C_SubscriberName.Loop2110C_SubscriberEligibilityOrBenefitInfo.Loop2120C_BenefitRelatedEntityGrouping.Loop2120C_BenefitRelatedEntity;
using EdiSource._271_5010.Loop2000C_SubscriberLevel.Loop2100C_SubscriberName.Loop2110C_SubscriberEligibilityOrBenefitInfo.Loop2120C_BenefitRelatedEntityGrouping.Segments;

namespace EdiSource._271_5010.Loop2000C_SubscriberLevel.Loop2100C_SubscriberName.Loop2110C_SubscriberEligibilityOrBenefitInfo.Loop2120C_BenefitRelatedEntityGrouping;

[LoopGenerator<_271_5010_Loop2110C_SubscriberEligibilityOrBenefitInfo, _271_5010_Loop2120C_BenefitRelatedEntityGrouping, _271_5010_Loop2120C_LS_LoopStart>]
public partial class _271_5010_Loop2120C_BenefitRelatedEntityGrouping : IValidatable
{
    [SegmentHeader] public _271_5010_Loop2120C_LS_LoopStart? LS_LoopStart { get; set; }

    [LoopList] public LoopList<_271_5010_Loop2120C_BenefitRelatedEntity> Loop2120C_BenefitRelatedEntities { get; set; } = [];

    [SegmentFooter] public _271_5010_Loop2120C_LE_LoopEnd? LE_LoopEnd { get; set; }

    public IEnumerable<ValidationMessage> Validate()
    {
        yield break;
    }
}
