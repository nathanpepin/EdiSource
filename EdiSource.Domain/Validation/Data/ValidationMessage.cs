using System.Text;

namespace EdiSource.Domain.Validation.Data;

public sealed class ValidationMessage
{
    public required ValidationSeverity ValidationSeverity { get; set; }
    public required string Message { get; set; }

    public string Subject { get; set; } = "Unknown";

    public int? LoopLine { get; set; }

    public int? SegmentLine { get; set; }

    public string? Loop { get; set; }
    public string? Segment { get; set; }
    public int? DataElement { get; set; }
    public int? CompositeElement { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append($"Severity: {ValidationSeverity}, Message: {Message}");

        sb.Append($", Subject: {Subject}");

        if (Loop != null)
        {
            sb.Append($", Loop: {Loop}");
        }

        if (Segment != null)
        {
            sb.Append($", Segment: {Segment}");
        }

        if (DataElement.HasValue)
        {
            sb.Append($", DataElement: {DataElement}");
        }

        if (CompositeElement.HasValue)
        {
            sb.Append($", CompositeElement: {CompositeElement}");
        }

        if (LoopLine.HasValue)
        {
            sb.Append($", LoopLine: {LoopLine}");
        }

        if (SegmentLine.HasValue)
        {
            sb.Append($", SegmentLine: {SegmentLine}");
        }

        return sb.ToString();
    }
}