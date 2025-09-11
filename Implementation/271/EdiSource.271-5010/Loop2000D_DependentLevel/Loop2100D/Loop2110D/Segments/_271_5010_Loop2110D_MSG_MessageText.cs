using EdiSource._271_5010.Loop2000D_DependentLevel.Loop2100D_DependentName.Loop2110D_DependentEligibilityOrBenefitInfo;

namespace EdiSource._271_5010.Loop2000D_DependentLevel.Loop2100D.Loop2110D.Segments;

[SegmentGenerator<_271_5010_Loop2110D_DependentEligibilityOrBenefitInfo>("MSG")]
public sealed partial class _271_5010_Loop2110D_MSG_MessageText
{
    public string? FreeFormMessageText { get => GetCompositeElement(1); set => SetCompositeElement(value, 1); }
    public string? PrinterCarriageControlCode { get => GetCompositeElement(2); set => SetCompositeElement(value, 2); }
    public int? Number { get => SegmentExtensions.GetInt(this, 3); set => this.SetInt(value, 3); }
}
