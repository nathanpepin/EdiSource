namespace EdiSource._271_5010.Loop2000C_SubscriberLevel.Loop2100C_SubscriberName.Segments;

[SegmentGenerator<_271_5010_Loop2100C_SubscriberName>("MP")]
public partial class _271_5010_Loop2100C_MP_SubscriberMilitaryPersonnelInfo
{
    public string? MilitaryServiceRankCode { get => GetCompositeElement(1); set => SetCompositeElement(value, 1); }
    public string? MilitaryServiceBranchCode { get => GetCompositeElement(2); set => SetCompositeElement(value, 2); }
    public string? MilitaryServiceRankCode2 { get => GetCompositeElement(3); set => SetCompositeElement(value, 3); }
    public string? MilitaryServiceRankCode3 { get => GetCompositeElement(4); set => SetCompositeElement(value, 4); }
    public string? MilitaryServiceRankCode4 { get => GetCompositeElement(5); set => SetCompositeElement(value, 5); }
    public string? DateTimePeriodFormatQualifier { get => GetCompositeElement(6); set => SetCompositeElement(value, 6); }
    public string? DateTimePeriod { get => GetCompositeElement(7); set => SetCompositeElement(value, 7); }
}
