namespace EdiSource.Domain.Validation.SourceGeneration;

/// <summary>
///     Validates that a composite element's value is one of an allowed set of values.
/// </summary>
/// <param name="validationSeverity">The severity level to report when validation fails.</param>
/// <param name="dataElement">The zero-based data element index.</param>
/// <param name="compositeElement">The zero-based composite element index.</param>
/// <param name="values">The allowed values.</param>
[AttributeUsage(AttributeTargets.Class)]
public sealed class IsOneOfValuesAttribute(
    ValidationSeverity validationSeverity,
    int dataElement,
    int compositeElement,
    params string[] values)
    : Attribute, IIndirectValidatable
{
    public IEnumerable<ValidationMessage> Validate(IEdi element)
    {
        if (element is not Segment segment)
            throw new ArgumentException("Element must be a segment", nameof(element));

        if (segment.GetCompositeElementOrNull(dataElement, compositeElement) is { } value
            && !values.Contains(value))
            yield return ValidationFactory.Create(
                segment,
                validationSeverity,
                $"Element {dataElement} in composite {compositeElement} must be one of: {string.Join(", ", values)}",
                dataElement,
                compositeElement);
    }
}