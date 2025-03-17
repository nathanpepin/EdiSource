using EdiSource.Domain.Elements;
using EdiSource.Domain.Separator;

namespace EdiSource.Domain.Tests.Elements;

[TestSubject(typeof(Element))]
public class ElementCompositeTests
{
    private readonly Separators _separators = new()
    {
        SegmentSeparator = '~',
        DataElementSeparator = '*',
        CompositeElementSeparator = ':'
    };

    [Fact]
    public void Constructor_WithMultipleCompositeElements_ShouldInitializeCorrectly()
    {
        // Arrange & Act
        var element = new Element(["FIRST", "SECOND", "THIRD"]);

        // Assert
        element.Should().HaveCount(3);
        element[0].Should().Be("FIRST");
        element[1].Should().Be("SECOND");
        element[2].Should().Be("THIRD");
    }

    [Fact]
    public void ImplicitConversion_FromStringArray_ShouldCreateElementWithMultipleCompositeElements()
    {
        // Arrange & Act
        Element element = new[] { "FIRST", "SECOND", "THIRD" };

        // Assert
        element.Should().HaveCount(3);
        element[0].Should().Be("FIRST");
        element[1].Should().Be("SECOND");
        element[2].Should().Be("THIRD");
    }

    [Fact]
    public void ImplicitConversion_ToStringArray_ShouldReturnAllCompositeElements()
    {
        // Arrange
        var element = new Element(["FIRST", "SECOND", "THIRD"]);

        // Act
        string[] values = element;

        // Assert
        values.Should().HaveCount(3);
        values[0].Should().Be("FIRST");
        values[1].Should().Be("SECOND");
        values[2].Should().Be("THIRD");
    }

    [Fact]
    public void ImplicitConversion_ToSingleString_ShouldReturnFirstCompositeElement()
    {
        // Arrange
        var element = new Element(["FIRST", "SECOND", "THIRD"]);

        // Act
        string value = element;

        // Assert
        value.Should().Be("FIRST");
    }

    [Fact]
    public void Add_CompositeElement_ShouldAddToElement()
    {
        // Arrange
        var element = new Element(["FIRST", "SECOND"]);

        // Act
        element.Add("THIRD");

        // Assert
        element.Should().HaveCount(3);
        element[2].Should().Be("THIRD");
    }

    [Fact]
    public void Insert_CompositeElement_ShouldInsertAtSpecifiedPosition()
    {
        // Arrange
        var element = new Element(["FIRST", "THIRD"]);

        // Act
        element.Insert(1, "SECOND");

        // Assert
        element.Should().HaveCount(3);
        element[0].Should().Be("FIRST");
        element[1].Should().Be("SECOND");
        element[2].Should().Be("THIRD");
    }

    [Fact]
    public void Remove_CompositeElement_ShouldRemoveMatchingElement()
    {
        // Arrange
        var element = new Element(["FIRST", "SECOND", "THIRD"]);

        // Act
        var result = element.Remove("SECOND");

        // Assert
        result.Should().BeTrue();
        element.Should().HaveCount(2);
        element[0].Should().Be("FIRST");
        element[1].Should().Be("THIRD");
    }

    [Fact]
    public void Remove_NonExistingCompositeElement_ShouldReturnFalse()
    {
        // Arrange
        var element = new Element(["FIRST", "SECOND", "THIRD"]);

        // Act
        var result = element.Remove("FOURTH");

        // Assert
        result.Should().BeFalse();
        element.Should().HaveCount(3);
    }

    [Fact]
    public void RemoveAt_ValidIndex_ShouldRemoveElementAtPosition()
    {
        // Arrange
        var element = new Element(["FIRST", "SECOND", "THIRD"]);

        // Act
        element.RemoveAt(1);

        // Assert
        element.Should().HaveCount(2);
        element[0].Should().Be("FIRST");
        element[1].Should().Be("THIRD");
    }

    [Fact]
    public void Clear_ShouldRemoveAllCompositeElements()
    {
        // Arrange
        var element = new Element(["FIRST", "SECOND", "THIRD"]);

        // Act
        element.Clear();

        // Assert
        element.Should().BeEmpty();
        element.Count.Should().Be(0);
    }

    [Fact]
    public void FromString_WithCompositeElements_ShouldCreateCorrectElements()
    {
        // Arrange
        var text = "NM1*IL*1*DOE:JOHN*A";

        // Act
        var elements = Element.FromString(text, _separators);

        // Assert
        elements.Should().HaveCount(5);
        elements[0][0].Should().Be("NM1");
        elements[1][0].Should().Be("IL");
        elements[2][0].Should().Be("1");
        elements[3][0].Should().Be("DOE");
        elements[3][1].Should().Be("JOHN");
        elements[3].Count.Should().Be(2); // This element has two composite elements
    }

    [Fact]
    public void IndexOf_ExistingCompositeElement_ShouldReturnCorrectIndex()
    {
        // Arrange
        var element = new Element(["FIRST", "SECOND", "THIRD"]);

        // Act
        var index = element.IndexOf("SECOND");

        // Assert
        index.Should().Be(1);
    }

    [Fact]
    public void IndexOf_NonExistingCompositeElement_ShouldReturnMinusOne()
    {
        // Arrange
        var element = new Element(["FIRST", "SECOND", "THIRD"]);

        // Act
        var index = element.IndexOf("FOURTH");

        // Assert
        index.Should().Be(-1);
    }

    [Fact]
    public void Contains_ExistingCompositeElement_ShouldReturnTrue()
    {
        // Arrange
        var element = new Element(["FIRST", "SECOND", "THIRD"]);

        // Act
        var contains = element.Contains("SECOND");

        // Assert
        contains.Should().BeTrue();
    }

    [Fact]
    public void Contains_NonExistingCompositeElement_ShouldReturnFalse()
    {
        // Arrange
        var element = new Element(["FIRST", "SECOND", "THIRD"]);

        // Act
        var contains = element.Contains("FOURTH");

        // Assert
        contains.Should().BeFalse();
    }

    [Fact]
    public void Equals_WithIdenticalCompositeElements_ShouldReturnTrue()
    {
        // Arrange
        var element1 = new Element(["FIRST", "SECOND", "THIRD"]);
        var element2 = new Element(["FIRST", "SECOND", "THIRD"]);

        // Act & Assert
        element1.Equals(element2).Should().BeTrue();
        (element1 == element2).Should().BeTrue();
    }

    [Fact]
    public void Equals_WithDifferentCompositeElements_ShouldReturnFalse()
    {
        // Arrange
        var element1 = new Element(["FIRST", "SECOND", "THIRD"]);
        var element2 = new Element(["FIRST", "DIFFERENT", "THIRD"]);

        // Act & Assert
        element1.Equals(element2).Should().BeFalse();
        (element1 == element2).Should().BeFalse();
    }

    [Fact]
    public void Equals_WithDifferentNumberOfCompositeElements_ShouldReturnFalse()
    {
        // Arrange
        var element1 = new Element(["FIRST", "SECOND", "THIRD"]);
        var element2 = new Element(["FIRST", "SECOND"]);

        // Act & Assert
        element1.Equals(element2).Should().BeFalse();
        (element1 == element2).Should().BeFalse();
    }

    [Fact]
    public void CopyTo_ShouldCopyElementsToArray()
    {
        // Arrange
        var element = new Element(["FIRST", "SECOND", "THIRD"]);
        var array = new string[5];

        // Act
        element.CopyTo(array, 1);

        // Assert
        array[0].Should().BeNull();
        array[1].Should().Be("FIRST");
        array[2].Should().Be("SECOND");
        array[3].Should().Be("THIRD");
        array[4].Should().BeNull();
    }
}