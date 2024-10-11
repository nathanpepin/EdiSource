using EdiSource.Domain.Segments;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.Factory;

namespace EdiSource.Domain.Validation.SourceGeneration;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ElementLengthAttribute(int dataElement, int compositeElement, int min, int max)
    : Attribute, IIndirectValidatable
{
    public ElementLengthAttribute(int dataElement, int compositeElement, int length)
        : this(dataElement, compositeElement, length, length)
    {
    }

    public IEnumerable<ValidationMessage> Validate(ISegment segment)
    {
        if (segment.GetCompositeElementOrNull(dataElement, compositeElement) is { } value
            && value.Length < min
            && value.Length > max)
        {
            yield return ValidationFactory.CreateWarning(
                segment,
                $"Element {dataElement} in composite {compositeElement} has length {value.Length} which is not between {min} and {max}",
                dataElement,
                compositeElement);
        }
    }
}

/*
[AttributeUsage(AttributeTargets.Class)]
   public sealed class ElementLengthAttribute(int dataElement, int compositeElement, int min, int max)
       : Attribute;

[AttributeUsage(AttributeTargets.Class)]
   public sealed class EmptyAttribute(int dataElement, int compositeElement)
       : Attribute;

[AttributeUsage(AttributeTargets.Class)]
   public sealed class HasLengthAttribute(int dataElement, int compositeElement, int length)
       : Attribute;

[AttributeUsage(AttributeTargets.Class)]
   public sealed class IsOneOfValuesAttribute(int dataElement, int compositeElement, params string[] values)
       : Attribute;

[AttributeUsage(AttributeTargets.Class)]
   public sealed class NotEmptyAttribute(int dataElement, int compositeElement)
       : Attribute;

[AttributeUsage(AttributeTargets.Class)]
   public sealed class NotOneOfValuesAttribute(int dataElement, int compositeElement, params string[] values)
       : Attribute;

[AttributeUsage(AttributeTargets.Class)]
   public sealed class RequiredDataElements(int[] dataElements)
       : Attribute;

[AttributeUsage(AttributeTargets.Class)]
   public sealed class SegmentElementLengthAttribute(int min, int max)
       : Attribute;
*/