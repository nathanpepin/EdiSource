namespace EdiSource._271_5010.TransactionSet.Segments;

[SegmentGenerator<_271_5010_EligibilityBenefitResponse>("SE")]
public sealed partial class _271_5010_SE_TransactionSetTrailer
{
    public int? TransactionSegmentCount { get => this.GetInt(1); set => this.SetInt(value, 1); }
    public string? TransactionSetControlNumber { get => GetCompositeElement(2); set => SetCompositeElement(value, 2); }
}
