namespace EdiSource.Domain.Validation.SourceGeneration;

/// <summary>
///     Validates that a composite element's string length falls within a specified range.
/// </summary>
/// <param name="validationSeverity">The severity level to report when validation fails.</param>
/// <param name="dataElement">The zero-based data element index.</param>
/// <param name="compositeElement">The zero-based composite element index.</param>
/// <param name="min">The minimum allowed length (inclusive).</param>
/// <param name="max">The maximum allowed length (inclusive).</param>
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
               (value.Length < min ||
               value.Length > max)
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