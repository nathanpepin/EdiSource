namespace EdiSource.Domain.Validation.SourceGeneration;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class BeDateTimeAttribute(
    ValidationSeverity validationSeverity,
    int dataElement,
    int compositeElement,
    string? format = null)
    : Attribute, IIndirectValidatable
{
    public IEnumerable<ValidationMessage> Validate(IEdi element)
    {
        if (element is not Segment segment)
            throw new ArgumentException("Element must be a segment", nameof(element));

        if (segment.GetCompositeElementOrNull(dataElement, compositeElement) is not { } value)
            yield break;

        var formats = format is null
            ? DateFormats.StandardFormats
            : [format];

        foreach (var _ in formats
                     .Select(f => DateOnly.TryParseExact(value, f, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                     .Where(result => result))
            yield break;

        yield return ValidationFactory.Create(
            segment,
            validationSeverity,
            $"Data element {dataElement} in composite element {compositeElement} should be a date but is not",
            dataElement);
    }
}