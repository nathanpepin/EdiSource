namespace EdiSource.Domain.Validation.SourceGeneration;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class ElementLengthAttribute(
    ValidationSeverity validationSeverity,
    int dataElement,
    int min,
    int max)
    : Attribute, IIndirectValidatable
{
    public ElementLengthAttribute(ValidationSeverity validationSeverity, int dataElement, int length)
        : this(validationSeverity, dataElement, length, length)
    {
    }

    public IEnumerable<ValidationMessage> Validate(IEdi element)
    {
        if (element is not Segment segment)
            throw new ArgumentException("Element must be a segment", nameof(element));

        if (segment.GetElementOrNull(dataElement) is not { } ce)
            return [];

        var sumLength = ce.Sum(x => x.Length);

        return sumLength < min || sumLength > max
            ? [CreateValidationMessage(segment, sumLength)]
            : [];
    }

    private ValidationMessage CreateValidationMessage(Segment segment, int value)
    {
        return ValidationFactory.Create(
            segment,
            validationSeverity,
            $"Element {dataElement}  has length {value} which is not between {min} and {max}",
            dataElement);
    }
}