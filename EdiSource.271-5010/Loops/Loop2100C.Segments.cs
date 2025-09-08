namespace EdiSource._271_5010.Loops;

[SegmentGenerator<Loop2100C>("NM1", "IL")]
public partial class Loop2100C_NM1
{
    public string EntityIdentifierCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string EntityTypeQualifier
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string SubscriberLastName
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string SubscriberFirstName
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }

    public string SubscriberMiddleNameOrInitial
    {
        get => GetCompositeElement(5);
        set => SetCompositeElement(value, 5);
    }

    public string SubscriberNameSuffix
    {
        get => GetCompositeElement(7);
        set => SetCompositeElement(value, 7);
    }

    public string IdentificationCodeQualifier
    {
        get => GetCompositeElement(8);
        set => SetCompositeElement(value, 8);
    }

    public string SubscriberPrimaryIdentifier
    {
        get => GetCompositeElement(9);
        set => SetCompositeElement(value, 9);
    }
}

[SegmentGenerator<Loop2100C>("REF")]
public partial class Loop2100C_REF
{
    public string ReferenceIdentificationQualifier
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string SubscriberSupplementalIdentifier
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

[SegmentGenerator<Loop2100C>("N3")]
public partial class Loop2100C_N3
{
    public string SubscriberAddressLine1
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string SubscriberAddressLine2
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }
}

[SegmentGenerator<Loop2100C>("N4")]
public partial class Loop2100C_N4
{
    public string SubscriberCityName
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string SubscriberStateCode
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string SubscriberPostalZoneOrZipCode
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string SubscriberCountryCode
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }

    public string SubscriberCountrySubdivisionCode
    {
        get => GetCompositeElement(7);
        set => SetCompositeElement(value, 7);
    }
}

[SegmentGenerator<Loop2100C>("AAA")]
public partial class Loop2100C_AAA
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

[SegmentGenerator<Loop2100C>("PRV")]
public partial class Loop2100C_PRV
{
    public string ProviderCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string ReferenceIdentificationQualifier
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string ProviderIdentifier
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }
}

[SegmentGenerator<Loop2100C>("DMG")]
public partial class Loop2100C_DMG
{
    public string DateTimePeriodFormatQualifier
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string SubscriberBirthDate
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string SubscriberGenderCode
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }
}

[SegmentGenerator<Loop2100C>("INS")]
public partial class Loop2100C_INS
{
    public string InsuredIndicator
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string IndividualRelationshipCode
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string MaintenanceTypeCode
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string MaintenanceReasonCode
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }

    public string BirthSequenceNumber
    {
        get => GetCompositeElement(17);
        set => SetCompositeElement(value, 17);
    }
}

[SegmentGenerator<Loop2100C>("HI")]
public partial class Loop2100C_HI
{
    public string HealthCareCodeInformation1
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string HealthCareCodeInformation2
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string HealthCareCodeInformation3
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string HealthCareCodeInformation4
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }

    public string HealthCareCodeInformation5
    {
        get => GetCompositeElement(5);
        set => SetCompositeElement(value, 5);
    }

    public string HealthCareCodeInformation6
    {
        get => GetCompositeElement(6);
        set => SetCompositeElement(value, 6);
    }

    public string HealthCareCodeInformation7
    {
        get => GetCompositeElement(7);
        set => SetCompositeElement(value, 7);
    }

    public string HealthCareCodeInformation8
    {
        get => GetCompositeElement(8);
        set => SetCompositeElement(value, 8);
    }
}

[SegmentGenerator<Loop2100C>("DTP")]
public partial class Loop2100C_DTP
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

    public string DateTimePeriod
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }
}

[SegmentGenerator<Loop2100C>("MPI")]
public partial class Loop2100C_MPI
{
    public string InformationStatusCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string EmploymentStatusCode
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string GovernmentServiceAffiliationCode
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string Description
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }

    public string MilitaryServiceRankCode
    {
        get => GetCompositeElement(5);
        set => SetCompositeElement(value, 5);
    }

    public string DateTimePeriodFormatQualifier
    {
        get => GetCompositeElement(6);
        set => SetCompositeElement(value, 6);
    }

    public string DateTimePeriod
    {
        get => GetCompositeElement(7);
        set => SetCompositeElement(value, 7);
    }
}