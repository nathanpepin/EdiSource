namespace EdiSource.Domain.Validation.SourceGeneration;

/// <summary>
///     Validates that a composite element is empty (zero length).
/// </summary>
/// <param name="validationSeverity">The severity level to report when validation fails.</param>
/// <param name="dataElement">The zero-based data element index.</param>
/// <param name="compositeElement">The zero-based composite element index.</param>
[AttributeUsage(AttributeTargets.Class)]
public sealed class EmptyAttribute(ValidationSeverity validationSeverity, int dataElement, int compositeElement)
    : Attribute, IIndirectValidatable
{
    public IEnumerable<ValidationMessage> Validate(IEdi element)
    {
        if (element is not Segment segment)
            throw new ArgumentException("Element must be a segment", nameof(element));

        if (segment.GetCompositeElementOrNull(dataElement, compositeElement) is { } value
            && value.Length != 0)
            yield return ValidationFactory.Create(
                segment,
                validationSeverity,
                $"Element {dataElement} in composite {compositeElement} should be empty",
                dataElement,
                compositeElement);
    }
}