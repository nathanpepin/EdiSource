namespace EdiSource._270_5010.Loop2000C_Subscriber.Loop2100C.Loop2110C.Segments;

[SegmentGenerator<_270_5010_Loop2110C_EligibilityBenefitInquiry>("AMT")]
public sealed partial class _270_5010_Loop2110C_AMT_SpendDownTotalBilled
{
    public string? AmountQualifierCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }
    
    public decimal? MonetaryAmount
    {
        get => this.GetDecimal(2);
        set => this.SetDecimal(value, 2);
    }
    
    public string? CreditDebitFlagCode
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }
}