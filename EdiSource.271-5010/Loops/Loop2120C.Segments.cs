namespace EdiSource._271_5010.Loops;

[SegmentGenerator<Loop2120CGrouping>("LS")]
public partial class Loop2120C_LS
{
    public string LoopIdentifierCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }
}

[SegmentGenerator<Loop2120C>("NM1")]
public partial class Loop2120C_NM1
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

    public string BenefitRelatedEntityRelationshipCode
    {
        get => GetCompositeElement(10);
        set => SetCompositeElement(value, 10);
    }
}

[SegmentGenerator<Loop2120C>("N3")]
public partial class Loop2120C_N3
{
    public string BenefitRelatedEntityAddressLine1
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string BenefitRelatedEntityAddressLine2
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }
}

[SegmentGenerator<Loop2120C>("N4")]
public partial class Loop2120C_N4
{
    public string BenefitRelatedEntityCityName
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string BenefitRelatedEntityStateCode
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string BenefitRelatedEntityPostalZoneOrZipCode
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string BenefitRelatedEntityCountryCode
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }

    public string BenefitRelatedEntityLocationQualifier
    {
        get => GetCompositeElement(5);
        set => SetCompositeElement(value, 5);
    }

    public string BenefitRelatedEntityDodHealthServiceRegion
    {
        get => GetCompositeElement(6);
        set => SetCompositeElement(value, 6);
    }

    public string BenefitRelatedEntityCountrySubdivisionCode
    {
        get => GetCompositeElement(7);
        set => SetCompositeElement(value, 7);
    }
}

[SegmentGenerator<Loop2120C>("PER")]
public partial class Loop2120C_PER
{
    public string ContactFunctionCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string BenefitRelatedEntityContactName
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string CommunicationNumberQualifier1
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string BenefitRelatedEntityCommunicationNumber1
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }

    public string CommunicationNumberQualifier2
    {
        get => GetCompositeElement(5);
        set => SetCompositeElement(value, 5);
    }

    public string BenefitRelatedEntityCommunicationNumber2
    {
        get => GetCompositeElement(6);
        set => SetCompositeElement(value, 6);
    }

    public string CommunicationNumberQualifier3
    {
        get => GetCompositeElement(7);
        set => SetCompositeElement(value, 7);
    }

    public string BenefitRelatedEntityCommunicationNumber3
    {
        get => GetCompositeElement(8);
        set => SetCompositeElement(value, 8);
    }
}

[SegmentGenerator<Loop2120C>("PRV")]
public partial class Loop2120C_PRV
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

    public string BenefitRelatedEntityProviderTaxonomyCode
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }
}

[SegmentGenerator<Loop2120CGrouping>("LE")]
public partial class Loop2120C_LE
{
    public string LoopIdentifierCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }
}