using EdiSource._271_5010.Loops;

namespace EdiSource._271_5010.Segments;

[SegmentGenerator<Loop2110C>("EB")]
public partial class Loop2110C_EB
{
    public string EligibilityOrBenefitInformation
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string BenefitCoverageLevelCode
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string ServiceTypeCode
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string InsuranceTypeCode
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }

    public string PlanCoverageDescription
    {
        get => GetCompositeElement(5);
        set => SetCompositeElement(value, 5);
    }

    public string TimePeriodQualifier
    {
        get => GetCompositeElement(6);
        set => SetCompositeElement(value, 6);
    }

    public string BenefitAmount
    {
        get => GetCompositeElement(7);
        set => SetCompositeElement(value, 7);
    }

    public string BenefitPercent
    {
        get => GetCompositeElement(8);
        set => SetCompositeElement(value, 8);
    }

    public string QuantityQualifier
    {
        get => GetCompositeElement(9);
        set => SetCompositeElement(value, 9);
    }

    public string BenefitQuantity
    {
        get => GetCompositeElement(10);
        set => SetCompositeElement(value, 10);
    }

    public string AuthorizationOrCertificationIndicator
    {
        get => GetCompositeElement(11);
        set => SetCompositeElement(value, 11);
    }

    public string InPlanNetworkIndicator
    {
        get => GetCompositeElement(12);
        set => SetCompositeElement(value, 12);
    }

    public string CompositeMedicalProcedureIdentifier
    {
        get => GetCompositeElement(13);
        set => SetCompositeElement(value, 13);
    }

    public string CompositeDiagnosisCodePointer
    {
        get => GetCompositeElement(14);
        set => SetCompositeElement(value, 14);
    }
}

[SegmentGenerator<Loop2110C>("HSD")]
public partial class Loop2110C_HSD
{
    public string QuantityQualifier
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string BenefitQuantity
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string UnitOrBasisForMeasurementCode
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string SampleSelectionModulus
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }

    public string TimePeriodQualifier
    {
        get => GetCompositeElement(5);
        set => SetCompositeElement(value, 5);
    }

    public string PeriodCount
    {
        get => GetCompositeElement(6);
        set => SetCompositeElement(value, 6);
    }

    public string DeliveryFrequencyCode
    {
        get => GetCompositeElement(7);
        set => SetCompositeElement(value, 7);
    }

    public string DeliveryPatternTimeCode
    {
        get => GetCompositeElement(8);
        set => SetCompositeElement(value, 8);
    }
}

[SegmentGenerator<Loop2110C>("REF")]
public partial class Loop2110C_REF
{
    public string ReferenceIdentificationQualifier
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string SubscriberEligibilityOrBenefitIdentifier
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string PlanGroupOrPlanNetworkName
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }
}

[SegmentGenerator<Loop2110C>("DTP")]
public partial class Loop2110C_DTP
{
    public string DateTimeQualifier
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string DateTimePeriodFormatQualifier
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string EligibilityOrBenefitDateTimePeriod
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }
}

[SegmentGenerator<Loop2110C>("AAA")]
public partial class Loop2110C_AAA
{
    public string ValidRequestIndicator
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string RejectReasonCode
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string FollowUpActionCode
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }
}

[SegmentGenerator<Loop2110C>("MSG")]
public partial class Loop2110C_MSG
{
    public string FreeFormMessageText
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }
}