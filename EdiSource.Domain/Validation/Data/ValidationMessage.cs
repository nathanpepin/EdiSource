using System.Text;

namespace EdiSource.Domain.Validation.Data;

public sealed class ValidationMessage
{
    /// <summary>
    ///     The severity of the validation message
    /// </summary>
    public required ValidationSeverity Severity { get; set; }

    /// <summary>
    ///     The message, either containing information or an error
    /// </summary>
    public required string Message { get; set; }

    /// <summary>
    ///     The entity that produced the validation. Typically: "Segment", or "Loop".
    /// </summary>
    public ValidationSubject Subject { get; set; } = ValidationSubject.Unknown;

    /// <summary>
    ///     The loop line of the element in question
    /// </summary>
    public int? LoopLine { get; set; }

    /// <summary>
    ///     The segment line of the segment in question
    /// </summary>
    public int? SegmentLine { get; set; }

    /// <summary>
    ///     The name of the loop context
    /// </summary>
    public string? Loop { get; set; }

    /// <summary>
    ///     The segment data in question
    /// </summary>
    public string? Segment { get; set; }

    /// <summary>
    ///     The data element position in question
    /// </summary>
    public int? DataElement { get; set; }

    /// <summary>
    ///     The composite element position in question
    /// </summary>
    public int? CompositeElement { get; set; }

    public string? Value { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append($"Severity: {Severity}");

        sb.Append($", Subject: {Subject}");

        if (Loop != null) sb.Append($", Loop: {Loop}");

        if (LoopLine.HasValue) sb.Append($", LoopLine: {LoopLine}");

        if (SegmentLine.HasValue) sb.Append($", SegmentLine: {SegmentLine}");

        if (DataElement.HasValue) sb.Append($", DataElement: {DataElement}");

        if (CompositeElement.HasValue) sb.Append($", CompositeElement: {CompositeElement}");

        if (Value is not null) sb.Append($", Value: {Value}");

        sb.Append($" Message: {Message}");

        if (Segment != null) sb.Append($", Segment: {Segment}");

        return sb.ToString();
    }
}