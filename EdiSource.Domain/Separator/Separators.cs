namespace EdiSource.Domain.Separator;

public sealed partial class Separators
{
    public static readonly Separators DefaultSeparators = new('~', '*', ':');

    public Separators(char segmentSeparator, char dataElementSeparator, char compositeElementSeparator)
    {
        SegmentSeparator = segmentSeparator;
        DataElementSeparator = dataElementSeparator;
        CompositeElementSeparator = compositeElementSeparator;
    }

    public Separators()
    {
    }

    public char SegmentSeparator { get; set; } = '~';
    public char DataElementSeparator { get; set; } = '*';
    public char CompositeElementSeparator { get; set; } = ':';
}