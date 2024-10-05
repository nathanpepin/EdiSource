namespace EdiSource.Domain.Separator;

public partial class Separators
{
    public static Task<Separator.Separators> CreateFromISA(StreamReader streamReader)
    {
        streamReader.BaseStream.Position = 3;
        var dataElementSeparator = (char)streamReader.Peek();
        streamReader.DiscardBufferedData();

        streamReader.BaseStream.Position = 104;
        var compositeElementSeparator = (char)streamReader.Peek();
        streamReader.DiscardBufferedData();

        streamReader.BaseStream.Position = 105;
        var segmentSeparator = (char)streamReader.Peek();
        streamReader.DiscardBufferedData();

        streamReader.BaseStream.Position = 0;

        return Task.FromResult(new Separator.Separators(segmentSeparator, dataElementSeparator, compositeElementSeparator));
    }

    public static Separator.Separators CreateFromISA(ReadOnlySpan<char> ediText)
    {
        return new Separator.Separators(ediText[3], ediText[104], ediText[105]);
    }
}