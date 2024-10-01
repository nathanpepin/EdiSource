namespace EdiSource.Domain.Seperator;

public partial struct Separators(char segmentSeparator, char dataElementSeparator, char compositeElementSeparator)
{
    public static readonly Separators DefaultSeparators = new('~', '*', ':');

    public char SegmentSeparator { get; set; } = segmentSeparator;
    public char DataElementSeparator { get; set; } = dataElementSeparator;
    public char CompositeElementSeparator { get; set; } = compositeElementSeparator;
}