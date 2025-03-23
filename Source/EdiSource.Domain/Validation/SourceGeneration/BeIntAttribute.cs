namespace EdiSource.Domain.Validation.SourceGeneration;

[AttributeUsage(AttributeTargets.Class)]
public sealed class BeIntAttribute(
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
            && !int.TryParse(value, out _))
            yield return ValidationFactory.Create(
                segment,
                validationSeverity,
                $"Data element {dataElement} in composite element {compositeElement} should be a time but is not",
                dataElement);
    }
}