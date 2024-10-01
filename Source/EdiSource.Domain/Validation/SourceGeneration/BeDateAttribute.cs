namespace EdiSource.Domain.Validation.SourceGeneration;

/// <summary>
///     Validates that a composite element value is a valid date.
///     Uses standard EDI date formats unless a custom format is specified.
/// </summary>
/// <param name="validationSeverity">The severity level to report when validation fails.</param>
/// <param name="dataElement">The zero-based data element index.</param>
/// <param name="compositeElement">The zero-based composite element index.</param>
/// <param name="format">Optional date format string. When null, standard EDI date formats are tried.</param>
[AttributeUsage(AttributeTargets.Class)]
public sealed class BeDateAttribute(
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