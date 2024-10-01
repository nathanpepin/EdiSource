namespace EdiSource.Domain.Validation.SourceGeneration;

/// <summary>
///     Validates that the specified data elements exist (are not null) in the segment.
/// </summary>
/// <param name="validationSeverity">The severity level to report when validation fails.</param>
/// <param name="dataElements">The zero-based data element indices that are required.</param>
[AttributeUsage(AttributeTargets.Class)]
public sealed class RequiredDataElementsAttribute(ValidationSeverity validationSeverity, int[] dataElements)
    : Attribute, IIndirectValidatable
{
    public IEnumerable<ValidationMessage> Validate(IEdi element)
    {
        if (element is not Segment segment)
            throw new ArgumentException("Element must be a segment", nameof(element));

        foreach (var dataElement in dataElements)
            if (segment.GetCompositeElementOrNull(dataElement) is null)
                yield return ValidationFactory.Create(
                    segment,
                    validationSeverity,
                    $"Data element {dataElement} is required but does not exist",
                    dataElement);
    }
}