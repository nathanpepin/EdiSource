namespace EdiSource.Domain.Separator;

public partial class Separators
{
    /// <summary>
    ///     The minimum length of an ISA segment (106 characters: ISA + 16 data elements + separators).
    /// </summary>
    /// <remarks>
    ///     Exposed internally so that other classes (e.g., EdiCommon) can reuse
    ///     the same constant instead of duplicating the magic number.
    /// </remarks>
    internal const int IsaSegmentLength = 106;

    /// <summary>
    ///     Position of the data element separator within the ISA segment (immediately after "ISA").
    /// </summary>
    private const int IsaDataElementSeparatorIndex = 3;

    /// <summary>
    ///     Position of the composite element separator within the ISA segment (ISA16).
    /// </summary>
    private const int IsaCompositeElementSeparatorIndex = 104;

    /// <summary>
    ///     Position of the segment terminator within the ISA segment.
    /// </summary>
    private const int IsaSegmentTerminatorIndex = 105;

    public static Task<InvalidISAException?> IsInvalidISA(StreamReader streamReader)
    {
        if (streamReader.BaseStream.Length < IsaSegmentLength)
            return Task.FromResult<InvalidISAException?>(
                new InvalidISAException($"ISA is too short, must be at least {IsaSegmentLength} characters long."));

        streamReader.BaseStream.Position = 0;

        Span<char> isaBuffer = stackalloc char[3];
        var isa = streamReader.Read(isaBuffer);

        if (isa != 3 || isaBuffer[0] != 'I' || isaBuffer[1] != 'S' || isaBuffer[2] != 'A')
            return Task.FromResult<InvalidISAException?>(
                new InvalidISAException("Expected ISA segment identifier, but found: " +
                                        new string(isaBuffer)));

        streamReader.DiscardBufferedData();
        streamReader.BaseStream.Position = 0;

        return Task.FromResult<InvalidISAException?>(null);
    }

    /// <summary>
    ///     Parses out the separators from an ISA stream.
    /// </summary>
    /// <param name="streamReader"></param>
    /// <returns></returns>
    public static Task<Separators> CreateFromISA(StreamReader streamReader)
    {
        if (streamReader.BaseStream.Length < IsaSegmentLength)
        {
            throw new InvalidISAException($"ISA is too short, must be at least {IsaSegmentLength} characters long.");
        }

        streamReader.BaseStream.Position = 0;
        Span<char> chars = stackalloc char[IsaSegmentLength];
        streamReader.Read(chars);

        if (chars[0] != 'I' || chars[1] != 'S' || chars[2] != 'A')
        {
            throw new InvalidISAException("Expected ISA segment, but none found.");
        }

        var dataElementSeparator = chars[IsaDataElementSeparatorIndex];
        var compositeElementSeparator = chars[IsaCompositeElementSeparatorIndex];
        var segmentSeparator = chars[IsaSegmentTerminatorIndex];

        streamReader.DiscardBufferedData();
        streamReader.BaseStream.Position = 0;

        return Task.FromResult(new Separators(segmentSeparator, dataElementSeparator, compositeElementSeparator));
    }

    /// <summary>
    ///     Parses out the separator values from an ISA text span.
    /// </summary>
    /// <param name="ediText"></param>
    /// <returns></returns>
    public static Separators CreateFromISA(ReadOnlySpan<char> ediText)
    {
        if (ediText.Length < IsaSegmentLength)
            throw new InvalidISAException($"ISA is too short, must be at least {IsaSegmentLength} characters long.");

        return new Separators(
            ediText[IsaSegmentTerminatorIndex],
            ediText[IsaDataElementSeparatorIndex],
            ediText[IsaCompositeElementSeparatorIndex]);
    }
}