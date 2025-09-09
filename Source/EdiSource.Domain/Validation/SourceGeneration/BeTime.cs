namespace EdiSource.Domain.Validation.SourceGeneration;

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
                $"Data element {dataElement} in composite element {compositeElement} is required but does not exist",
                dataElement);
    }
}