namespace EdiSource.Domain.Separator;

/// <summary>
///     The <c>Separators</c> class represents the characters used to delimit different parts of an EDI message.
/// </summary>
public sealed partial class Separators
{
    /// <summary>
    ///     A static instance of <see cref="Separators" /> initialized with default EDI separators.
    ///     The <see cref="SegmentSeparator" /> is '~', the <see cref="DataElementSeparator" /> is '*',
    ///     and the <see cref="CompositeElementSeparator" /> is ':'.
    ///     <br /><br />
    ///     These values can be changed during application start if other defaults are wanted.
    /// </summary>
    public static readonly Separators DefaultSeparators = new('~', '*', ':');

    /// This class provides properties to store segment, data element, and composite element separators.
    /// Additionally, it includes methods to create instances of the Separators class using the ISA segment of an EDI document.
    public Separators(char segmentSeparator, char dataElementSeparator, char compositeElementSeparator)
    {
        SegmentSeparator = segmentSeparator;
        DataElementSeparator = dataElementSeparator;
        CompositeElementSeparator = compositeElementSeparator;
    }

    public Separators()
    {
    }

    /// <summary>
    ///     Gets or sets the character used to separate segments in an EDI message.
    /// </summary>
    /// <value>
    ///     The character used to separate segments from other segments. Default value is '~'.
    /// </value>
    public char SegmentSeparator { get; set; } = '~';

    /// <summary>
    ///     Gets or sets the character used to separate individual data elements within a segment.
    /// </summary>
    /// <value>
    ///     The character used to separate data elements within a segment element. Default value is '*'.
    /// </value>
    public char DataElementSeparator { get; set; } = '*';

    /// <summary>
    ///     Gets or sets the character used to separate composite elements within a data element.
    /// </summary>
    /// <value>
    ///     The character used to separate composite elements within a data element. Default value is ':'.
    /// </value>
    public char CompositeElementSeparator { get; set; } = ':';
}