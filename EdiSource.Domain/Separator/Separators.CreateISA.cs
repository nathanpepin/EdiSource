namespace EdiSource.Domain.Separator;

public partial class Separators
{
    /// <summary>
    ///     Parses out the segments from an ISA stream
    /// </summary>
    /// <param name="streamReader"></param>
    /// <returns></returns>
    public static Task<Separators> CreateFromISA(StreamReader streamReader)
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

        return Task.FromResult(new Separators(segmentSeparator, dataElementSeparator, compositeElementSeparator));
    }

    /// <summary>
    ///     Parses out the segment values from an ISA text span
    /// </summary>
    /// <param name="ediText"></param>
    /// <returns></returns>
    public static Separators CreateFromISA(ReadOnlySpan<char> ediText)
    {
        return new Separators(ediText[105], ediText[3], ediText[104]);
    }
}