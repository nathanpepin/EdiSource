namespace EdiSource.Domain.Validation.SourceGeneration;

/// <summary>
///     Validates that a composite element value is a valid decimal number.
/// </summary>
/// <param name="validationSeverity">The severity level to report when validation fails.</param>
/// <param name="dataElement">The zero-based data element index.</param>
/// <param name="compositeElement">The zero-based composite element index.</param>
[AttributeUsage(AttributeTargets.Class)]
public sealed class BeDecimalAttribute(
    ValidationSeverity validationSeverity,
    int dataElement,
    int compositeElement)
    : Attribute, IIndirectValidatable
{
    public IEnumerable<ValidationMessage> Validate(IEdi element)
    {
        if (element is not Segment segment)
            throw new ArgumentException("Element must be a segment", nameof(element));

        if (segment.GetCompositeElementOrNull(dataElement, compositeElement) is { } value
            && !decimal.TryParse(value, out _))
            yield return ValidationFactory.Create(
                segment,
                validationSeverity,
                $"Data element {dataElement} in composite element {compositeElement} should be a decimal but is not",
                dataElement);
    }
}