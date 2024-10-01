namespace EdiSource.SampleApp.Segments;

[SegmentGenerator<Loop2100>("PER")]
public partial class Loop2100_PER
{
    public string ContactFunction
    {
        get => GetCompositeElement(1);
        set => SetCompositeElement(value, 1);
    }

    public string? ContactName
    {
        get => GetCompositeElementOrNull(2);
        set => value?.Do(x => SetCompositeElement(x, 2));
    }

    public string? CommunicationQualifier1
    {
        get => GetCompositeElementOrNull(3);
        set => value?.Do(x => SetCompositeElement(x, 3));
    }

    public string? CommunicationNumber1
    {
        get => GetCompositeElementOrNull(4);
        set => value?.Do(x => SetCompositeElement(x, 4));
    }
}