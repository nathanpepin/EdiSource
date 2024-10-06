using EdiSource.Domain.Segments;
using EdiSource.Domain.Validation.Data;

namespace EdiSource.Domain.Validation.Factory;

public static partial class ValidationFactory
{
    public static ValidationMessage Create(ISegment segment, ValidationSeverity validationSeverity,
        string message, int? dataElement = null, int? compositeElement = null) => new()
    {
        Severity = validationSeverity,
        Subject = "Segment",
        Message = message,
        Loop = segment.Parent?.GetType().Name,
        Segment = segment.ToString(),
        DataElement = dataElement,
        CompositeElement = compositeElement
    };

    public static ValidationMessage CreateCritical(ISegment segment,
        string message, int? dataElement = null, int? compositeElement = null)
        => Create(segment, ValidationSeverity.Error, message, dataElement, compositeElement);

    public static ValidationMessage CreateError(ISegment segment,
        string message, int? dataElement = null, int? compositeElement = null)
        => Create(segment, ValidationSeverity.Error, message, dataElement, compositeElement);

    public static ValidationMessage CreateWarning(ISegment segment,
        string message, int? dataElement = null, int? compositeElement = null)
        => Create(segment, ValidationSeverity.Warning, message, dataElement, compositeElement);

    public static ValidationMessage CreateInfo(ISegment segment,
        string message, int? dataElement = null, int? compositeElement = null)
        => Create(segment, ValidationSeverity.Info, message, dataElement, compositeElement);
}