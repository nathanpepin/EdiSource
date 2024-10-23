using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Validation.Data;

namespace EdiSource.Domain.Validation.Factory;

public static partial class ValidationFactory
{
    public static ValidationMessage Create(ISegment segment, ValidationSeverity validationSeverity,
        string message, int? dataElement = null, int? compositeElement = null)
    {
        return new ValidationMessage
        {
            Severity = validationSeverity,
            Subject = ValidationSubject.Segment,
            Message = message,
            Loop = null,// segment.Parent ? parent.Parent?.GetType().Name : null,
            Segment = segment.ToString(),
            DataElement = dataElement,
            CompositeElement = compositeElement,
            Value = dataElement is { } de && compositeElement is { } ce
                ? segment.GetCompositeElementOrNull(de, ce)
                : null
        };
    }

    public static ValidationMessage CreateCritical(ISegment segment,
        string message, int? dataElement = null, int? compositeElement = null)
    {
        return Create(segment, ValidationSeverity.Error, message, dataElement, compositeElement);
    }

    public static ValidationMessage CreateError(ISegment segment,
        string message, int? dataElement = null, int? compositeElement = null)
    {
        return Create(segment, ValidationSeverity.Error, message, dataElement, compositeElement);
    }

    public static ValidationMessage CreateWarning(ISegment segment,
        string message, int? dataElement = null, int? compositeElement = null)
    {
        return Create(segment, ValidationSeverity.Warning, message, dataElement, compositeElement);
    }

    public static ValidationMessage CreateInfo(ISegment segment,
        string message, int? dataElement = null, int? compositeElement = null)
    {
        return Create(segment, ValidationSeverity.Info, message, dataElement, compositeElement);
    }
}