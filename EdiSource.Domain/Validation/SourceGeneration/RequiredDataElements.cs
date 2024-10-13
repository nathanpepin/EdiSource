using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.Factory;

namespace EdiSource.Domain.Validation.SourceGeneration;

[AttributeUsage(AttributeTargets.Class)]
public sealed class RequiredDataElementsAttribute(ValidationSeverity validationSeverity, int[] dataElements)
    : Attribute, IIndirectValidatable
{
    public IEnumerable<ValidationMessage> Validate(IEdi element)
    {
        if (element is not Segment segment)
            throw new ArgumentException("Element must be a segment", nameof(element));

        foreach (var dataElement in dataElements)
        {
            if (segment.GetCompositeElementOrNull(dataElement, 0) is null)
            {
                yield return ValidationFactory.Create(
                    segment,
                    validationSeverity,
                    $"Data element {dataElement} is required but does not exist",
                    dataElement);
            }
        }
    }
}