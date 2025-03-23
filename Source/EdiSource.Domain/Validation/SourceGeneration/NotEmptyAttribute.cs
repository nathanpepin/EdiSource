namespace EdiSource.Domain.Validation.SourceGeneration;

[AttributeUsage(AttributeTargets.Class)]
public sealed class NotEmptyAttribute(ValidationSeverity validationSeverity, int dataElement, int compositeElement)
    : Attribute, IIndirectValidatable
{
    public IEnumerable<ValidationMessage> Validate(IEdi element)
    {
        if (element is not Segment segment)
            throw new ArgumentException("Element must be a segment", nameof(element));

        if (segment.GetCompositeElementOrNull(dataElement, compositeElement) is { Length: 0 })
            yield return ValidationFactory.Create(
                segment,
                validationSeverity,
                $"Element {dataElement} in composite {compositeElement} has should not be empty",
                dataElement,
                compositeElement);
    }
}