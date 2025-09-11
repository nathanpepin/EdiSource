namespace EdiSource._270_5010.Loop2000C_Subscriber.Loop2100C.Segments;

[SegmentGenerator<_270_5010_Loop2100C_SubscriberName>("INS")]
public sealed partial class _270_5010_Loop2100C_INS_MultipleBirthSequence
{
    public string? InsuredIndicator
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string? IndividualRelationshipCode
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string? MaintenanceTypeCode
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string? MaintenanceReasonCode
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }

    public string? BenefitStatusCode
    {
        get => GetCompositeElement(5);
        set => SetCompositeElement(value, 5);
    }

    public string? MedicareStatusCode
    {
        get => GetCompositeElement(6);
        set => SetCompositeElement(value, 6);
    }

    public string? ConsolidatedOmnibusBudgetReconciliationActCOBRAQualifyingEventCode
    {
        get => GetCompositeElement(7);
        set => SetCompositeElement(value, 7);
    }

    public string? EmploymentStatusCode
    {
        get => GetCompositeElement(8);
        set => SetCompositeElement(value, 8);
    }

    public string? StudentStatusCode
    {
        get => GetCompositeElement(9);
        set => SetCompositeElement(value, 9);
    }

    public string? HandicapIndicator
    {
        get => GetCompositeElement(10);
        set => SetCompositeElement(value, 10);
    }

    public DateOnly? DateOfDeath
    {
        get => this.GetDateOnly(11);
        set => this.SetDateOnly(value, 11);
    }

    public string? ConfidentialityCode
    {
        get => GetCompositeElement(12);
        set => SetCompositeElement(value, 12);
    }

    public int? BirthSequenceNumber
    {
        get => this.GetInt(17);
        set => this.SetInt(value, 17);
    }
}