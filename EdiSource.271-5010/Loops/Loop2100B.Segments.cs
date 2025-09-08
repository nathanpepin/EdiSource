namespace EdiSource._271_5010.Loops;

[SegmentGenerator<Loop2100B>("NM1", $"1P{S}2B{S}36{S}80{S}FA{S}GP{S}P5{S}PR")]
public partial class Loop2100B_NM1
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

    public string InformationReceiverLastOrOrganizationName
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string InformationReceiverFirstName
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }

    public string InformationReceiverMiddleName
    {
        get => GetCompositeElement(5);
        set => SetCompositeElement(value, 5);
    }

    public string InformationReceiverNameSuffix
    {
        get => GetCompositeElement(7);
        set => SetCompositeElement(value, 7);
    }

    public string IdentificationCodeQualifier
    {
        get => GetCompositeElement(8);
        set => SetCompositeElement(value, 8);
    }

    public string InformationReceiverIdentificationNumber
    {
        get => GetCompositeElement(9);
        set => SetCompositeElement(value, 9);
    }
}

[SegmentGenerator<Loop2100B>("REF")]
public partial class Loop2100B_REF
{
    public string ReferenceIdentificationQualifier
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string InformationReceiverAdditionalIdentifier
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string InformationReceiverAdditionalIdentifierState
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }
}

[SegmentGenerator<Loop2100B>("N3")]
public partial class Loop2100B_N3
{
    public string InformationReceiverAddressLine1
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string InformationReceiverAdditionalAddressLine
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }
}

[SegmentGenerator<Loop2100B>("N4")]
public partial class Loop2100B_N4
{
    public string InformationReceiverCityName
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string InformationReceiverStateCode
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string InformationReceiverPostalZoneOrZipCode
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string CountryCode
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }

    public string CountrySubdivisionCode
    {
        get => GetCompositeElement(7);
        set => SetCompositeElement(value, 7);
    }
}

[SegmentGenerator<Loop2100B>("AAA")]
public partial class Loop2100B_AAA
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

[SegmentGenerator<Loop2100B>("PRV")]
public partial class Loop2100B_PRV
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

    public string InformationReceiverProviderTaxonomyCode
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }
}