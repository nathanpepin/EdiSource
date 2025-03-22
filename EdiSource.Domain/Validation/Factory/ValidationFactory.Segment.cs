namespace EdiSource.Domain.Validation.Factory;

public static partial class ValidationFactory
{
    public static ValidationMessage Create(Segment segment, ValidationSeverity validationSeverity,
        string message, int? dataElement = null, int? compositeElement = null)
    {
        return new ValidationMessage
        {
            Severity = validationSeverity,
            Subject = ValidationSubject.Segment,
            Message = message,
            Loop = null, // segment.Parent ? parent.Parent?.GetType().Name : null,
            Segment = segment.ToString(),
            DataElement = dataElement,
            CompositeElement = compositeElement,
            Value = dataElement is { } de && compositeElement is { } ce
                ? segment.GetCompositeElementOrNull(de, ce)
                : null
        };
    }

    public static ValidationMessage CreateCritical(Segment segment,
        string message, int? dataElement = null, int? compositeElement = null)
    {
        return Create(segment, ValidationSeverity.Error, message, dataElement, compositeElement);
    }

    public static ValidationMessage CreateError(Segment segment,
        string message, int? dataElement = null, int? compositeElement = null)
    {
        return Create(segment, ValidationSeverity.Error, message, dataElement, compositeElement);
    }

    public static ValidationMessage CreateWarning(Segment segment,
        string message, int? dataElement = null, int? compositeElement = null)
    {
        return Create(segment, ValidationSeverity.Warning, message, dataElement, compositeElement);
    }

    public static ValidationMessage CreateInfo(Segment segment,
        string message, int? dataElement = null, int? compositeElement = null)
    {
        return Create(segment, ValidationSeverity.Info, message, dataElement, compositeElement);
    }
}