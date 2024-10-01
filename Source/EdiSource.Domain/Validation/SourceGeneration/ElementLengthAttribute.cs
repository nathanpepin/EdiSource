namespace EdiSource.Domain.Validation.SourceGeneration;

/// <summary>
///     Validates that the total length of all composite elements in a data element
///     falls within a specified range.
/// </summary>
/// <param name="validationSeverity">The severity level to report when validation fails.</param>
/// <param name="dataElement">The zero-based data element index.</param>
/// <param name="min">The minimum allowed total length (inclusive).</param>
/// <param name="max">The maximum allowed total length (inclusive).</param>
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
            $"Element {dataElement} has length {value} which is not between {min} and {max}",
            dataElement);
    }
}