namespace EdiSource.Domain.Validation.SourceGeneration;

/// <summary>
///     Validates that a specific composite element exists (is not null) in the segment.
/// </summary>
/// <param name="validationSeverity">The severity level to report when validation fails.</param>
/// <param name="dataElement">The zero-based data element index.</param>
/// <param name="compositeElement">The zero-based composite element index.</param>
[AttributeUsage(AttributeTargets.Class)]
public sealed class RequiredElementAttribute(
    ValidationSeverity validationSeverity,
    int dataElement,
    int compositeElement)
    : Attribute, IIndirectValidatable
{
    public IEnumerable<ValidationMessage> Validate(IEdi element)
    {
        if (element is not Segment segment)
            throw new ArgumentException("Element must be a segment", nameof(element));

        if (segment.GetCompositeElementOrNull(dataElement, compositeElement) is null)
            yield return ValidationFactory.Create(
                segment,
                validationSeverity,
                $"Data element {dataElement} in composite element {compositeElement} is required but does not exist",
                dataElement);
    }
}