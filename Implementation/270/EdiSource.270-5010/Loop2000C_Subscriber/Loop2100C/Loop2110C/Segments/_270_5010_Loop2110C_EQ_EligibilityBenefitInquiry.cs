namespace EdiSource._270_5010.Loop2000C_Subscriber.Loop2100C.Loop2110C.Segments;

[SegmentGenerator<_270_5010_Loop2110C_EligibilityBenefitInquiry>("EQ")]
public sealed partial class _270_5010_Loop2110C_EQ_EligibilityBenefitInquiry
{
    public string? ServiceTypeCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string? CompositeMedicalProcedureIdentifier
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string? CoverageLevelCode
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string? InsuranceTypeCode
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }

    public string? CompositeDiagnosisCodePointer
    {
        get => GetCompositeElement(5);
        set => SetCompositeElement(value, 5);
    }
}