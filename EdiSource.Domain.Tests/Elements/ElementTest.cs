using FluentAssertions;
using EdiSource.Domain.Elements;
using EdiSource.Domain.Separator;
using JetBrains.Annotations;

namespace EdiSource.Domain.Tests.Elements;

[TestSubject(typeof(Element))]
public class ElementTests
{
    private readonly Separators _separators = new()
    {
        SegmentSeparator = '~',
        DataElementSeparator = '*',
        CompositeElementSeparator = ':'
    };

    [Fact]
    public void Constructor_WithSingleValue_ShouldCreateElementWithOneValue()
    {
        // Arrange & Act
        var element = new Element(["TEST"]);

        // Assert
        element.Should().HaveCount(1);
        element[0].Should().Be("TEST");
    }

    [Fact]
    public void Constructor_WithMultipleValues_ShouldCreateElementWithAllValues()
    {
        // Arrange & Act
        var element = new Element(["TEST1", "TEST2", "TEST3"]);

        // Assert
        element.Should().HaveCount(3);
        element[0].Should().Be("TEST1");
        element[1].Should().Be("TEST2");
        element[2].Should().Be("TEST3");
    }

    [Fact]
    public void ImplicitConversion_FromString_ShouldCreateElementWithSingleValue()
    {
        // Arrange & Act
        Element element = "TEST";

        // Assert
        element.Should().HaveCount(1);
        element[0].Should().Be("TEST");
    }

    [Fact]
    public void ImplicitConversion_ToStringArray_ShouldReturnAllValues()
    {
        // Arrange
        Element element = new(["TEST1", "TEST2"]);

        // Act
        string[] values = element;

        // Assert
        values.Should().HaveCount(2);
        values[0].Should().Be("TEST1");
        values[1].Should().Be("TEST2");
    }

    [Theory]
    [InlineData("ABC*DEF*GHI", 3)]
    [InlineData("ABC", 1)]
    [InlineData("ABC*DEF:GHI*JKL", 3)]
    public void FromString_WithValidInput_ShouldCreateCorrectElements(string input, int expectedCount)
    {
        // Arrange & Act
        var elements = Element.FromString(input, _separators);

        // Assert
        elements.Should().HaveCount(expectedCount);
    }

    [Fact]
    public void GetDate_WithValidFormat_ShouldReturnCorrectDate()
    {
        // Arrange
        Element element = "20240101";

        // Act
        var date = element.GetDate(format: "yyyyMMdd");

        // Assert
        date.Should().NotBeNull();
        date.Should().Be(new DateTime(2024, 1, 1));
    }

    [Fact]
    public void GetInt_WithValidNumber_ShouldReturnCorrectValue()
    {
        // Arrange
        Element element = "123";

        // Act
        var number = element.GetInt();

        // Assert
        number.Should().Be(123);
    }

    [Fact]
    public void GetDecimal_WithValidNumber_ShouldReturnCorrectValue()
    {
        // Arrange
        Element element = "123.45";

        // Act
        var number = element.GetDecimal();

        // Assert
        number.Should().Be(123.45m);
    }

    [Fact]
    public void Equals_WithIdenticalElements_ShouldReturnTrue()
    {
        // Arrange
        var element1 = new Element(["TEST1", "TEST2"]);
        var element2 = new Element(["TEST1", "TEST2"]);

        // Act & Assert
        element1.Should().BeEquivalentTo(element2);
        (element1 == element2).Should().BeTrue();
    }

    [Theory]
    [InlineData(TestEnum.Success, "Success")]
    [InlineData(TestEnum.Failure, "Failure")]
    public void GetEnumeration_WithValidValue_ShouldReturnCorrectEnum(TestEnum expected, string value)
    {
        // Arrange
        Element element = value;

        // Act
        var result = element.GetEnum<TestEnum>();

        // Assert
        result.Should().Be(expected);
    }

    public enum TestEnum
    {
        Success,
        Failure
    }
}