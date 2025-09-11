namespace EdiSource._270_5010.TransactionSet.Segments;

[SegmentGenerator<_270_5010_EligibilityBenefitInquiry>("SE")]
public sealed partial class _270_5010_SE_TransactionSetTrailer
{
    public int? TransactionSegmentCount
    {
        get => this.GetInt(1);
        set => this.SetInt(value, 1);
    }

    public string? TransactionSetControlNumber
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }
}