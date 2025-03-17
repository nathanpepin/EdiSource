using EdiSource.Domain.Elements;

namespace EdiSource.Domain.Tests.Elements;

[TestSubject(typeof(Element))]
public class ElementConversionTests
{
    [Fact]
    public void GetDate_WithValidFormat_ShouldReturnCorrectDate()
    {
        // Arrange
        Element element = "20240315";

        // Act
        var result = element.GetDate();

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(new DateTime(2024, 03, 15));
    }

    [Fact]
    public void GetDate_WithInvalidFormat_ShouldReturnNull()
    {
        // Arrange
        Element element = "Invalid-Date";

        // Act
        var result = element.GetDate();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetDate_WithCustomFormat_ShouldReturnCorrectDate()
    {
        // Arrange
        Element element = "03/15/2024";

        // Act
        var result = element.GetDate(format: "MM/dd/yyyy");

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(new DateTime(2024, 03, 15));
    }

    [Fact]
    public void GetDate_WithCompositeElement_ShouldReturnCorrectDate()
    {
        // Arrange
        var element = new Element(["NotADate", "20240315"]);

        // Act
        var result = element.GetDate(compositeElement: 1);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(new DateTime(2024, 03, 15));
    }

    [Fact]
    public void GetInt_WithValidNumber_ShouldReturnCorrectValue()
    {
        // Arrange
        Element element = "12345";

        // Act
        var result = element.GetInt();

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(12345);
    }

    [Fact]
    public void GetInt_WithInvalidNumber_ShouldReturnNull()
    {
        // Arrange
        Element element = "Not-A-Number";

        // Act
        var result = element.GetInt();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetInt_WithCompositeElement_ShouldReturnCorrectValue()
    {
        // Arrange
        var element = new Element(["Not-A-Number", "12345"]);

        // Act
        var result = element.GetInt(compositeElement: 1);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(12345);
    }

    [Fact]
    public void GetDecimal_WithValidNumber_ShouldReturnCorrectValue()
    {
        // Arrange
        Element element = "123.45";

        // Act
        var result = element.GetDecimal();

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(123.45m);
    }

    [Fact]
    public void GetDecimal_WithInvalidNumber_ShouldReturnNull()
    {
        // Arrange
        Element element = "Not-A-Decimal";

        // Act
        var result = element.GetDecimal();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetDecimal_WithCompositeElement_ShouldReturnCorrectValue()
    {
        // Arrange
        var element = new Element(["Not-A-Decimal", "123.45"]);

        // Act
        var result = element.GetDecimal(compositeElement: 1);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(123.45m);
    }

    [Fact]
    public void GetBool_WithTrueValue_ShouldReturnTrue()
    {
        // Arrange
        Element element = "Y";

        // Act
        var result = element.GetBool("Y");

        // Assert
        result.Should().NotBeNull();
        result.Should().BeTrue();
    }

    [Fact]
    public void GetBool_WithFalseValue_ShouldReturnFalse()
    {
        // Arrange
        Element element = "N";

        // Act
        var result = element.GetBool("Y", falseValue: "N");

        // Assert
        result.Should().NotBeNull();
        result.Should().BeFalse();
    }

    [Fact]
    public void GetBool_WithNeitherTrueNorFalseValue_AndImplicitFalse_ShouldReturnFalse()
    {
        // Arrange
        Element element = "X";

        // Act
        var result = element.GetBool("Y", implicitFalse: true);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeFalse();
    }

    [Fact]
    public void GetBool_WithNeitherTrueNorFalseValue_AndNotImplicitFalse_ShouldReturnNull()
    {
        // Arrange
        Element element = "X";

        // Act
        var result = element.GetBool("Y", implicitFalse: false);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetBool_WithCompositeElement_ShouldReturnCorrectValue()
    {
        // Arrange
        var element = new Element(["X", "Y"]);

        // Act
        var result = element.GetBool("Y", compositeElement: 1);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(TestEnum.Value1, "Value1")]
    [InlineData(TestEnum.Value2, "Value2")]
    [InlineData(TestEnum.Value3, "Value3")]
    public void GetEnum_WithValidValue_ShouldReturnCorrectEnum(TestEnum expected, string value)
    {
        // Arrange
        Element element = value;

        // Act
        var result = element.GetEnum<TestEnum>();

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(expected);
    }

    [Fact]
    public void GetEnum_WithInvalidValue_ShouldReturnNull()
    {
        // Arrange
        Element element = "InvalidEnum";

        // Act
        var result = element.GetEnum<TestEnum>();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetEnum_WithUnderscorePrefix_ShouldReturnCorrectEnum()
    {
        // Arrange
        Element element = "Abc";

        // Act
        var result = element.GetEnum<TestEnumWithUnderscore>();

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(TestEnumWithUnderscore._Abc);
    }

    [Fact]
    public void GetEnum_WithoutTryAddUnderscore_ShouldReturnNull()
    {
        // Arrange
        Element element = "Abc";

        // Act
        var result = element.GetEnum<TestEnumWithUnderscore>(tryAddUnderscore: false);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetDateOnly_WithValidFormat_ShouldReturnCorrectDateOnly()
    {
        // Arrange
        Element element = "20240315";

        // Act
        var result = element.GetDateOnly();

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(new DateOnly(2024, 03, 15));
    }

    [Fact]
    public void GetDateOnly_WithInvalidFormat_ShouldReturnNull()
    {
        // Arrange
        Element element = "Invalid-Date";

        // Act
        var result = element.GetDateOnly();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetTimeOnly_WithValidFormat_ShouldReturnCorrectTimeOnly()
    {
        // Arrange
        Element element = "1430";

        // Act
        var result = element.GetTimeOnly();

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(new TimeOnly(14, 30));
    }

    [Fact]
    public void GetTimeOnly_WithInvalidFormat_ShouldReturnNull()
    {
        // Arrange
        Element element = "Invalid-Time";

        // Act
        var result = element.GetTimeOnly();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetTimeOnly_WithCustomFormat_ShouldReturnCorrectTimeOnly()
    {
        // Arrange
        Element element = "14:30:45";

        // Act
        var result = element.GetTimeOnly(format: "HH:mm:ss");

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(new TimeOnly(14, 30, 45));
    }

    // Test enums for testing
    public enum TestEnum
    {
        Value1,
        Value2,
        Value3
    }

    public enum TestEnumWithUnderscore
    {
        _Abc,
        _Def
    }
}