namespace EdiSource._271_5010.Loop2000C_SubscriberLevel.Loop2100C_SubscriberName.Loop2110C_SubscriberEligibilityOrBenefitInfo.Segments;

[SegmentGenerator<_271_5010_Loop2110C_SubscriberEligibilityOrBenefitInfo>("MSG")]
public partial class _271_5010_Loop2110C_MSG_MessageText
{
    public string? FreeFormMessageText { get => GetCompositeElement(1); set => SetCompositeElement(value, 1); }
    public string? PrinterCarriageControlCode { get => GetCompositeElement(2); set => SetCompositeElement(value, 2); }
    public int? Number { get => this.GetInt(3); set => this.SetInt(value, 3); }
}
