namespace EdiSource._271_5010.Loops;

[SegmentGenerator<Loop2100D>("NM1")]
public partial class Loop2100D_NM1
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

    public string DependentLastName
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string DependentFirstName
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }

    public string DependentMiddleName
    {
        get => GetCompositeElement(5);
        set => SetCompositeElement(value, 5);
    }

    public string DependentNameSuffix
    {
        get => GetCompositeElement(7);
        set => SetCompositeElement(value, 7);
    }
}

[SegmentGenerator<Loop2100D>("REF")]
public partial class Loop2100D_REF
{
    public string ReferenceIdentificationQualifier
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string DependentSupplementalIdentifier
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

[SegmentGenerator<Loop2100D>("N3")]
public partial class Loop2100D_N3
{
    public string DependentAddressLine1
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string DependentAddressLine2
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }
}

[SegmentGenerator<Loop2100D>("N4")]
public partial class Loop2100D_N4
{
    public string DependentCityName
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string DependentStateCode
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string DependentPostalZoneOrZipCode
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string DependentCountryCode
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }

    public string DependentCountrySubdivisionCode
    {
        get => GetCompositeElement(7);
        set => SetCompositeElement(value, 7);
    }
}

[SegmentGenerator<Loop2100D>("AAA")]
public partial class Loop2100D_AAA
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

[SegmentGenerator<Loop2100D>("PRV")]
public partial class Loop2100D_PRV
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

[SegmentGenerator<Loop2100D>("DMG")]
public partial class Loop2100D_DMG
{
    public string DateTimePeriodFormatQualifier
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string DependentBirthDate
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string DependentGenderCode
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }
}

[SegmentGenerator<Loop2100D>("INS")]
public partial class Loop2100D_INS
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

[SegmentGenerator<Loop2100D>("HI")]
public partial class Loop2100D_HI
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

[SegmentGenerator<Loop2100D>("DTP")]
public partial class Loop2100D_DTP
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

[SegmentGenerator<Loop2100D>("MPI")]
public partial class Loop2100D_MPI
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