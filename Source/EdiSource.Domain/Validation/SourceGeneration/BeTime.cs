namespace EdiSource.Domain.Validation.SourceGeneration;

/// <summary>
///     Validates that a composite element value is a valid time.
/// </summary>
/// <param name="validationSeverity">The severity level to report when validation fails.</param>
/// <param name="dataElement">The zero-based data element index.</param>
/// <param name="compositeElement">The zero-based composite element index.</param>
/// <param name="format">The time format string (default: "HHmm").</param>
[AttributeUsage(AttributeTargets.Class)]
public sealed class BeTimeAttribute(
    ValidationSeverity validationSeverity,
    int dataElement,
    int compositeElement,
    string format = "HHmm")
    : Attribute, IIndirectValidatable
{
    public IEnumerable<ValidationMessage> Validate(IEdi element)
    {
        if (element is not Segment segment)
            throw new ArgumentException("Element must be a segment", nameof(element));

        if (segment.GetCompositeElementOrNull(dataElement, compositeElement) is { } value
            && !TimeOnly.TryParseExact(value, format, null, DateTimeStyles.None, out _))
            yield return ValidationFactory.Create(
                segment,
                validationSeverity,
                $"Data element {dataElement} in composite element {compositeElement} should be a valid time but is not",
                dataElement);
    }
}