namespace EdiSource._271_5010.Loops;

[SegmentGenerator<Loop2120DGrouping>("LS")]
public partial class Loop2120D_LS
{
    public string LoopIdentifierCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }
}

[SegmentGenerator<Loop2120D>("NM1")]
public partial class Loop2120D_NM1
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

    public string LastOrOrganizationName
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string FirstName
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }

    public string MiddleName
    {
        get => GetCompositeElement(5);
        set => SetCompositeElement(value, 5);
    }

    public string NameSuffix
    {
        get => GetCompositeElement(7);
        set => SetCompositeElement(value, 7);
    }

    public string IdentificationCodeQualifier
    {
        get => GetCompositeElement(8);
        set => SetCompositeElement(value, 8);
    }

    public string BenefitRelatedEntityIdentifier
    {
        get => GetCompositeElement(9);
        set => SetCompositeElement(value, 9);
    }

    public string EntityRelationshipCode
    {
        get => GetCompositeElement(10);
        set => SetCompositeElement(value, 10);
    }
}

[SegmentGenerator<Loop2120D>("N3")]
public partial class Loop2120D_N3
{
    public string AddressLine1
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string AddressLine2
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }
}

[SegmentGenerator<Loop2120D>("N4")]
public partial class Loop2120D_N4
{
    public string CityName
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string StateCode
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string PostalCode
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string CountryCode
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }

    public string LocationQualifier
    {
        get => GetCompositeElement(5);
        set => SetCompositeElement(value, 5);
    }

    public string DodHealthServiceRegion
    {
        get => GetCompositeElement(6);
        set => SetCompositeElement(value, 6);
    }

    public string CountrySubdivisionCode
    {
        get => GetCompositeElement(7);
        set => SetCompositeElement(value, 7);
    }
}

[SegmentGenerator<Loop2120D>("PER")]
public partial class Loop2120D_PER
{
    public string ContactFunctionCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string ContactName
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string CommunicationNumberQualifier1
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string CommunicationNumber1
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }

    public string CommunicationNumberQualifier2
    {
        get => GetCompositeElement(5);
        set => SetCompositeElement(value, 5);
    }

    public string CommunicationNumber2
    {
        get => GetCompositeElement(6);
        set => SetCompositeElement(value, 6);
    }

    public string CommunicationNumberQualifier3
    {
        get => GetCompositeElement(7);
        set => SetCompositeElement(value, 7);
    }

    public string CommunicationNumber3
    {
        get => GetCompositeElement(8);
        set => SetCompositeElement(value, 8);
    }
}

[SegmentGenerator<Loop2120D>("PRV")]
public partial class Loop2120D_PRV
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

    public string ProviderTaxonomyCode
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }
}

[SegmentGenerator<Loop2120DGrouping>("LE")]
public partial class Loop2120D_LE
{
    public string LoopIdentifierCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }
}