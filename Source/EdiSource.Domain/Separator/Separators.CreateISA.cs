namespace EdiSource.Domain.Separator;

public partial class Separators
{
    public static Task<InvalidISAException?> IsInvalidISA(StreamReader streamReader)
    {
        if (streamReader.BaseStream.Length < 106) return Task.FromResult<InvalidISAException?>(new InvalidISAException("ISA is too short, must be at least 106 characters long."));

        streamReader.BaseStream.Position = 0;

        Span<char> isaBuffer = stackalloc char[3];
        var isa = streamReader.Read(isaBuffer);

        if (isa != 3 || isaBuffer[0] != 'I' || isaBuffer[1] != 'S' || isaBuffer[2] != 'A')
            return Task.FromResult<InvalidISAException?>(new InvalidISAException("ISA is too short, must be at least 106 characters long."));

        streamReader.DiscardBufferedData();
        streamReader.BaseStream.Position = 0;

        return Task.FromResult<InvalidISAException?>(null);
    }

    /// <summary>
    ///     Parses out the segments from an ISA stream
    /// </summary>
    /// <param name="streamReader"></param>
    /// <returns></returns>
    public static Task<Separators> CreateFromISA(StreamReader streamReader)
    {
        if (streamReader.BaseStream.Length < 106)
        {
            throw new InvalidISAException("ISA is too short, must be at least 106 characters long.");
        }
        
        streamReader.BaseStream.Position = 0;
        Span<char> chars = stackalloc char[106];
        streamReader.Read(chars);

        if (chars[0] != 'I' || chars[1] != 'S' || chars[2] != 'A')
        {
            throw new InvalidISAException("Expected ISA segment, but none found.");
        }
        
        var dataElementSeparator = chars[3];
        var compositeElementSeparator = chars[104];
        var segmentSeparator = chars[105];
        
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