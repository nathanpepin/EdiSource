namespace EdiSource.Domain.Validation.SourceGeneration;

[AttributeUsage(AttributeTargets.Class)]
public sealed class CompositeElementLengthAttribute(
    ValidationSeverity validationSeverity,
    int dataElement,
    int compositeElement,
    int min,
    int max)
    : Attribute, IIndirectValidatable
{
    public CompositeElementLengthAttribute(ValidationSeverity validationSeverity, int dataElement, int compositeElement,
        int length)
        : this(validationSeverity, dataElement, compositeElement, length, length)
    {
    }

    public IEnumerable<ValidationMessage> Validate(IEdi element)
    {
        if (element is not Segment segment)
            throw new ArgumentException("Element must be a segment", nameof(element));

        return segment.GetCompositeElementOrNull(dataElement, compositeElement) is { } value &&
               value.Length < min &&
               value.Length > max
            ? [CreateValidationMessage(segment, value.Length)]
            : [];
    }

    private ValidationMessage CreateValidationMessage(Segment segment, int value)
    {
        return ValidationFactory.Create(
            segment,
            validationSeverity,
            $"Element {dataElement} in composite {compositeElement} has length {value} which is not between {min} and {max}",
            dataElement,
            compositeElement);
    }
}