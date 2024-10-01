using EdiSource._271_5010.Loop2000C_SubscriberLevel.Loop2100C;

namespace EdiSource._271_5010.Loop2000C_SubscriberLevel.Loop2100C_SubscriberName.Segments;

[SegmentGenerator<_271_5010_Loop2100C_SubscriberName>("INS")]
public partial class _271_5010_Loop2100C_INS_SubscriberRelationship
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

    public bool? HandicapIndicator
    {
        get => this.GetBool(10, "Y", falseValue: "N");
        set => this.SetBool(value, "Y", "N", 10);
    }

    public string? DateTimePeriodFormatQualifier
    {
        get => GetCompositeElement(11);
        set => SetCompositeElement(value, 11);
    }

    public string? DateTimePeriod
    {
        get => GetCompositeElement(12);
        set => SetCompositeElement(value, 12);
    }
}