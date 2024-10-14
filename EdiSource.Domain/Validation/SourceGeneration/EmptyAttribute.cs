using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.Factory;

namespace EdiSource.Domain.Validation.SourceGeneration;

[AttributeUsage(AttributeTargets.Class)]
public sealed class EmptyAttribute(ValidationSeverity validationSeverity, int dataElement, int compositeElement)
    : Attribute, IIndirectValidatable
{
    public IEnumerable<ValidationMessage> Validate(IEdi element)
    {
        if (element is not Segment segment)
            throw new ArgumentException("Element must be a segment", nameof(element));

        if (segment.GetCompositeElementOrNull(dataElement, compositeElement) is { } value
            && value.Length != 0)
            yield return ValidationFactory.Create(
                segment,
                validationSeverity,
                $"Element {dataElement} in composite {compositeElement} has should be empty",
                dataElement,
                compositeElement);
    }
}