namespace EdiSource.Domain.Structure;

/// <summary>
///     The EdiTree class represents the hierarchical structure of EDI data segments and loops.
///     It may have some use in instances where the edi data format needs conversion into another format,
///     like html.
/// </summary>
public sealed partial class EdiTree
{
    /// <summary>
    ///     Represents the name or identifier of the loop in the EDI tree structure.
    /// </summary>
    public string Loop { get; set; } = string.Empty;

    /// <summary>
    ///     Represents the collection of segments associated with the current EDI (Electronic Data Interchange) tree node.
    /// </summary>
    /// <remarks>
    ///     Each segment in the list is a string that follows the structured format of EDI segments,
    ///     which can include various types of data elements separated by defined separators.
    /// </remarks>
    public List<string> Segments { get; } = [];

    /// <summary>
    ///     Gets or sets the nested loops within the current loop structure.
    /// </summary>
    /// <remarks>
    ///     The <c>Loops</c> property is a list of <see cref="EdiTree" /> objects representing
    ///     the hierarchical nature of EDI loop structures. Each <see cref="EdiTree" /> object within
    ///     the <c>Loops</c> collection can contain its own child loops and segments.
    /// </remarks>
    public List<EdiTree> Loops { get; } = [];
}