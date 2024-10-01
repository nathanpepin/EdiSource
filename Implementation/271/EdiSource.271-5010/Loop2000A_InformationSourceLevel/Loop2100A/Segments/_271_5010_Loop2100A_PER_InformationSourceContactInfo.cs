namespace EdiSource._271_5010.Loop2000A_InformationSourceLevel.Loop2100A.Segments;

[SegmentGenerator<_271_5010_Loop2100A_InformationSourceName>("PER")]
public sealed partial class _271_5010_Loop2100A_PER_InformationSourceContactInfo
{
    public string? ContactFunctionCode
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string? InformationSourceContactName
    {
        get => GetCompositeElement(2);
        set => SetCompositeElement(value, 2);
    }

    public string? CommunicationNumberQualifier1
    {
        get => GetCompositeElement(3);
        set => SetCompositeElement(value, 3);
    }

    public string? CommunicationNumber1
    {
        get => GetCompositeElement(4);
        set => SetCompositeElement(value, 4);
    }

    public string? CommunicationNumberQualifier2
    {
        get => GetCompositeElement(5);
        set => SetCompositeElement(value, 5);
    }

    public string? CommunicationNumber2
    {
        get => GetCompositeElement(6);
        set => SetCompositeElement(value, 6);
    }

    public string? CommunicationNumberQualifier3
    {
        get => GetCompositeElement(7);
        set => SetCompositeElement(value, 7);
    }

    public string? CommunicationNumber3
    {
        get => GetCompositeElement(8);
        set => SetCompositeElement(value, 8);
    }

    public string? ContactInquiryReference
    {
        get => GetCompositeElement(9);
        set => SetCompositeElement(value, 9);
    }
}