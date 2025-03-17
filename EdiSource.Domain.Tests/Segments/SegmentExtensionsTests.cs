using EdiSource.Domain.Exceptions;
using EdiSource.Domain.Segments.Extensions;

namespace EdiSource.Domain.Tests.Segments;

[TestSubject(typeof(SegmentExtensions))]
public class SegmentExtensionsTests
{
    [Fact]
    public void GetDate_WithValidFormat_ShouldReturnCorrectDate()
    {
        // Arrange
        var segment = new Segment("DTP*356*D8*20240315");

        // Act
        var result = segment.GetDate(3);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(new DateTime(2024, 03, 15));
    }

    [Fact]
    public void GetDate_WithInvalidFormat_ShouldReturnNull()
    {
        // Arrange
        var segment = new Segment("DTP*356*D8*InvalidDate");

        // Act
        var result = segment.GetDate(2);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetDateRequired_WithValidFormat_ShouldReturnCorrectDate()
    {
        // Arrange
        var segment = new Segment("DTP*356*D8*20240315");

        // Act
        var result = segment.GetDateRequired(3);

        // Assert
        result.Should().Be(new DateTime(2024, 03, 15));
    }

    [Fact]
    public void GetDateRequired_WithInvalidFormat_ShouldThrowException()
    {
        // Arrange
        var segment = new Segment("DTP*356*D8*InvalidDate");

        // Act & Assert
        Assert.Throws<DataElementParsingError>(() => segment.GetDateRequired(2));
    }

    [Fact]
    public void SetDate_WithDateTime_ShouldSetCorrectValue()
    {
        // Arrange
        var segment = new Segment("DTP*356*D8*");
        var date = new DateTime(2024, 03, 15);

        // Act
        var result = segment.SetDate(date, 2);

        // Assert
        result.Should().BeTrue();
        segment[2].Should().Be("20240315");
    }

    [Fact]
    public void SetDate_WithNullableDateTime_ShouldSetCorrectValue()
    {
        // Arrange
        var segment = new Segment("DTP*356*D8*");
        DateTime? date = new DateTime(2024, 03, 15);

        // Act
        var result = segment.SetDate(date, 2);

        // Assert
        result.Should().BeTrue();
        segment[2].Should().Be("20240315");
    }

    [Fact]
    public void SetDate_WithNullDateTimeValue_ShouldReturnFalse()
    {
        // Arrange
        var segment = new Segment("DTP*356*D8*20240315");
        DateTime? date = null;

        // Act
        var result = segment.SetDate(date, 3);

        // Assert
        result.Should().BeFalse();
        segment[3].Should().Be("20240315"); // Should not change the original value
    }

    [Fact]
    public void GetInt_WithValidNumber_ShouldReturnCorrectValue()
    {
        // Arrange
        var segment = new Segment("QTY*01*12345");

        // Act
        var result = segment.GetInt(2);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(12345);
    }

    [Fact]
    public void GetInt_WithInvalidNumber_ShouldReturnNull()
    {
        // Arrange
        var segment = new Segment("QTY*01*NotANumber");

        // Act
        var result = segment.GetInt(2);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetIntRequired_WithValidNumber_ShouldReturnCorrectValue()
    {
        // Arrange
        var segment = new Segment("QTY*01*12345");

        // Act
        var result = segment.GetIntRequired(2);

        // Assert
        result.Should().Be(12345);
    }

    [Fact]
    public void GetIntRequired_WithInvalidNumber_ShouldThrowException()
    {
        // Arrange
        var segment = new Segment("QTY*01*NotANumber");

        // Act & Assert
        Assert.Throws<DataElementParsingError>(() => segment.GetIntRequired(2));
    }

    [Fact]
    public void SetInt_WithValue_ShouldSetCorrectValue()
    {
        // Arrange
        var segment = new Segment("QTY*01*");
        var value = 12345;

        // Act
        var result = segment.SetInt(value, 1);

        // Assert
        result.Should().BeTrue();
        segment[1].Should().Be("12345");
    }

    [Fact]
    public void SetInt_WithNullableValue_ShouldSetCorrectValue()
    {
        // Arrange
        var segment = new Segment("QTY*01*");
        int? value = 12345;

        // Act
        var result = segment.SetInt(value, 1);

        // Assert
        result.Should().BeTrue();
        segment[1].Should().Be("12345");
    }

    [Fact]
    public void SetInt_WithNullValue_ShouldReturnFalse()
    {
        // Arrange
        var segment = new Segment("QTY*01*12345");
        int? value = null;

        // Act
        var result = segment.SetInt(value, 2);

        // Assert
        result.Should().BeFalse();
        segment[2].Should().Be("12345"); // Should not change the original value
    }

    [Fact]
    public void GetDecimal_WithValidNumber_ShouldReturnCorrectValue()
    {
        // Arrange
        var segment = new Segment("AMT*01*123.45");

        // Act
        var result = segment.GetDecimal(2);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(123.45m);
    }

    [Fact]
    public void GetDecimalRequired_WithInvalidNumber_ShouldThrowException()
    {
        // Arrange
        var segment = new Segment("AMT*01*NotADecimal");

        // Act & Assert
        Assert.Throws<DataElementParsingError>(() => segment.GetDecimalRequired(2));
    }

    [Fact]
    public void SetDecimal_WithValue_ShouldSetCorrectValue()
    {
        // Arrange
        var segment = new Segment("AMT*01*");
        var value = 123.45m;

        // Act
        var result = segment.SetDecimal(value, 1);

        // Assert
        result.Should().BeTrue();
        segment[1].Should().Be("123.45");
    }

    [Fact]
    public void GetBool_WithTrueValue_ShouldReturnTrue()
    {
        // Arrange
        var segment = new Segment("IND*Y");

        // Act
        var result = segment.GetBool(1, "Y");

        // Assert
        result.Should().NotBeNull();
        result.Should().BeTrue();
    }

    [Fact]
    public void GetBoolRequired_WithInvalidValue_ShouldThrowException()
    {
        // Arrange
        var segment = new Segment("IND*X");

        // Act & Assert
        Assert.Throws<DataElementParsingError>(() => segment.GetBoolRequired(0, "Y", implicitFalse: false));
    }

    [Fact]
    public void SetBool_WithBooleanValue_ShouldSetCorrectValue()
    {
        // Arrange
        var segment = new Segment("IND*");
        var value = true;

        // Act
        var result = segment.SetBool(value, "Y", "N", 0);

        // Assert
        result.Should().BeTrue();
        segment[0].Should().Be("Y");
    }

    [Fact]
    public void GetEnum_WithValidValue_ShouldReturnCorrectEnum()
    {
        // Arrange
        var segment = new Segment("TST*Monday");

        // Act
        var result = segment.GetEnum<DayOfWeek>(1);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(DayOfWeek.Monday);
    }

    [Fact]
    public void GetEnumRequired_WithInvalidValue_ShouldThrowException()
    {
        // Arrange
        var segment = new Segment("TST*NotADay");

        // Act & Assert
        Assert.Throws<DataElementParsingError>(() => segment.GetEnumRequired<DayOfWeek>(0));
    }

    [Fact]
    public void SetEnum_WithEnumValue_ShouldSetCorrectValue()
    {
        // Arrange
        var segment = new Segment("TST*");
        var value = DayOfWeek.Monday;

        // Act
        var result = segment.SetEnum(value, 1);

        // Assert
        result.Should().BeTrue();
        segment[1].Should().Be("Monday");
    }

    [Fact]
    public void GetDateOnly_WithValidFormat_ShouldReturnCorrectDateOnly()
    {
        // Arrange
        var segment = new Segment("DTP*356*D8*20240315");

        // Act
        var result = segment.GetDateOnly(3);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(new DateOnly(2024, 03, 15));
    }

    [Fact]
    public void SetDateOnly_WithNullableDateOnly_ShouldSetCorrectValue()
    {
        // Arrange
        var segment = new Segment("DTP*356*D8*");
        DateOnly? date = new DateOnly(2024, 03, 15);

        // Act
        var result = segment.SetDateOnly(date, 2);

        // Assert
        result.Should().BeTrue();
        segment[2].Should().Be("20240315");
    }

    [Fact]
    public void GetTimeOnly_WithValidFormat_ShouldReturnCorrectTimeOnly()
    {
        // Arrange
        var segment = new Segment("DTM*01*1430");

        // Act
        var result = segment.GetTimeOnly(2);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(new TimeOnly(14, 30));
    }

    [Fact]
    public void SetTimeOnly_WithTimeOnlyValue_ShouldSetCorrectValue()
    {
        // Arrange
        var segment = new Segment("DTM*01*");
        var time = new TimeOnly(14, 30);

        // Act
        var result = segment.SetTimeOnly(time, 1);

        // Assert
        result.Should().BeTrue();
        segment[1].Should().Be("1430");
    }

    [Fact]
    public void WriteToStringBuilder_WithSegment_ShouldOutputFormattedString()
    {
        // Arrange
        var segment = new Segment("ST*835*000000001");
        
        // Act
        var builder = segment.WriteToStringBuilder();
        var result = builder.ToString();
        
        // Assert
        result.Should().Be("ST*835*000000001~");
    }
}