using EdiSource._271_5010.Loop2000D_DependentLevel.Loop2100D_DependentName.Loop2110D_DependentEligibilityOrBenefitInfo;
using EdiSource._271_5010.Loop2000D_DependentLevel.Loop2100D.Loop2110D.Loop2120D.Loop2120D;
using EdiSource._271_5010.Loop2000D_DependentLevel.Loop2100D.Loop2110D.Loop2120D.Segments;

namespace EdiSource._271_5010.Loop2000D_DependentLevel.Loop2100D.Loop2110D.Loop2120D;

[LoopGenerator<_271_5010_Loop2110D_DependentEligibilityOrBenefitInfo, _271_5010_Loop2120D_BenefitRelatedEntityGrouping, _271_5010_Loop2120D_LS_LoopStart>]
public sealed partial class _271_5010_Loop2120D_BenefitRelatedEntityGrouping : IValidatable
{
    [SegmentHeader] public _271_5010_Loop2120D_LS_LoopStart? LS_LoopStart { get; set; }

    [LoopList] public LoopList<_271_5010_Loop2120D_BenefitRelatedEntity> Loop2120D_BenefitRelatedEntities { get; set; } = [];

    [SegmentFooter] public _271_5010_Loop2120D_LE_LoopEnd? LE_LoopEnd { get; set; }

    public IEnumerable<ValidationMessage> Validate()
    {
        yield break;
    }
}
