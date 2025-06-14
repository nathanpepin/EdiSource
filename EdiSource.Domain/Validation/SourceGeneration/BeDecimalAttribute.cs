using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.Factory;

namespace EdiSource.Domain.Validation.SourceGeneration;

[AttributeUsage(AttributeTargets.Class)]
public sealed class BeDecimalAttribute(
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
            && !decimal.TryParse(value, out _))
            yield return ValidationFactory.Create(
                segment,
                validationSeverity,
                $"Data element {dataElement} in composite element {compositeElement} should be a decimal but is not",
                dataElement);
    }
}