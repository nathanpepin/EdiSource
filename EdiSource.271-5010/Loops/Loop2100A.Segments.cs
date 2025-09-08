using EdiSource.Domain.Identifiers;

namespace EdiSource._271_5010.Loops;

[SegmentGenerator<Loop2100A>("NM1", $"2B{S}36{S}GP{S}P5{S}PR")]
public partial class Loop2100A_NM1
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

    public string InformationSourceLastOrOrganizationName
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string InformationSourceFirstName
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }

    public string InformationSourceMiddleName
    {
        get => GetCompositeElement(5);
        set => SetCompositeElement(value, 5);
    }

    public string InformationSourceNameSuffix
    {
        get => GetCompositeElement(7);
        set => SetCompositeElement(value, 7);
    }

    public string IdentificationCodeQualifier
    {
        get => GetCompositeElement(8);
        set => SetCompositeElement(value, 8);
    }

    public string InformationSourcePrimaryIdentifier
    {
        get => GetCompositeElement(9);
        set => SetCompositeElement(value, 9);
    }
}

[SegmentGenerator<Loop2100A>("PER")]
public partial class Loop2100A_PER
{
    public string ContactFunctionCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string InformationSourceContactName
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string CommunicationNumberQualifier1
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string InformationSourceCommunicationNumber1
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }

    public string CommunicationNumberQualifier2
    {
        get => GetCompositeElement(5);
        set => SetCompositeElement(value, 5);
    }

    public string InformationSourceCommunicationNumber2
    {
        get => GetCompositeElement(6);
        set => SetCompositeElement(value, 6);
    }

    public string CommunicationNumberQualifier3
    {
        get => GetCompositeElement(7);
        set => SetCompositeElement(value, 7);
    }

    public string InformationSourceCommunicationNumber3
    {
        get => GetCompositeElement(8);
        set => SetCompositeElement(value, 8);
    }
}

[SegmentGenerator<Loop2100A>("AAA")]
public partial class Loop2100A_AAA
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