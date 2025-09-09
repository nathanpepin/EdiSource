namespace EdiSource.Domain.Tests.Elements;

[TestSubject(typeof(Element))]
public class ElementEdgeCaseTests
{
    [Fact]
    public void Element_WithNullValues_ShouldCreateEmptyElement()
    {
        // Arrange & Act
        var element = new Element();

        // Assert
        element.Should().BeEmpty();
        element.Count.Should().Be(0);
    }

    [Fact]
    public void Element_WithEmptyArray_ShouldCreateEmptyElement()
    {
        // Arrange & Act
        var element = new Element(Array.Empty<string>());

        // Assert
        element.Should().BeEmpty();
        element.Count.Should().Be(0);
    }

    [Fact]
    public void GetDate_WithEmptyString_ShouldReturnNull()
    {
        // Arrange
        Element element = "";

        // Act
        var result = element.GetDate();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetInt_WithEmptyString_ShouldReturnNull()
    {
        // Arrange
        Element element = "";

        // Act
        var result = element.GetInt();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetDecimal_WithEmptyString_ShouldReturnNull()
    {
        // Arrange
        Element element = "";

        // Act
        var result = element.GetDecimal();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetValue_WithIndexOutOfRange_ShouldThrowException()
    {
        // Arrange
        Element element = "SingleValue";

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => element[1]);
    }

    [Fact]
    public void GetDate_WithOutOfRangeCompositeElement_ShouldReturnNull()
    {
        // Arrange
        Element element = "20240315";

        // Act
        var result = element.GetDate(1);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetInt_WithBoundaryValue_ShouldReturnCorrectValue()
    {
        // Arrange
        Element element = int.MaxValue.ToString();

        // Act
        var result = element.GetInt();

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(int.MaxValue);
    }

    [Fact]
    public void GetDecimal_WithBoundaryValue_ShouldReturnCorrectValue()
    {
        // Arrange
        Element element = decimal.MaxValue.ToString(CultureInfo.InvariantCulture);

        // Act
        var result = element.GetDecimal();

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(decimal.MaxValue);
    }

    [Fact]
    public void FromString_WithEmptyString_ShouldReturnEmptyElement()
    {
        // Arrange
        var separators = new Separators();

        // Act
        var elements = Element.FromString("", separators);

        // Assert
        elements.Should().HaveCount(1);
        elements[0].Should().HaveCount(1);
        elements[0][0].Should().BeEmpty();
    }

    [Fact]
    public void FromString_WithOnlySeparators_ShouldReturnEmptyElements()
    {
        // Arrange
        var separators = new Separators('~', '*', ':');

        // Act
        var elements = Element.FromString("*****", separators);

        // Assert
        elements.Should().HaveCount(6);
        foreach (var element in elements)
        {
            element.Should().HaveCount(1);
            element[0].Should().BeEmpty();
        }
    }

    [Fact]
    public void Element_WithEmptyStrings_ShouldMaintainEmptyStrings()
    {
        // Arrange & Act
        var element = new Element(["", "", ""]);

        // Assert
        element.Should().HaveCount(3);
        element[0].Should().BeEmpty();
        element[1].Should().BeEmpty();
        element[2].Should().BeEmpty();
    }

    [Fact]
    public void Element_WithNullElementInArray_ShouldThrowArgumentNullException()
    {
        // Arrange
        string[] values = ["Value1", null!, "Value3"];

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new Element(values));
    }

    [Fact]
    public void GetEnum_WithNullString_ShouldReturnNull()
    {
        // Arrange
        var element = new Element([""]);

        // Act
        var result = element.GetEnum<DayOfWeek>();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetEnum_WithEmptyString_ShouldReturnNull()
    {
        // Arrange
        Element element = "";

        // Act
        var result = element.GetEnum<DayOfWeek>();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetDateOnly_WithMaxPossibleDate_ShouldReturnCorrectDateOnly()
    {
        // Arrange
        Element element = "99991231"; // 9999-12-31

        // Act
        var result = element.GetDateOnly();

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(new DateOnly(9999, 12, 31));
    }

    [Fact]
    public void GetTimeOnly_WithMaxPossibleTime_ShouldReturnCorrectTimeOnly()
    {
        // Arrange
        Element element = "2359"; // 23:59

        // Act
        var result = element.GetTimeOnly();

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(new TimeOnly(23, 59));
    }
}