using EdiSource.Domain.Validation.SourceGeneration;
using JetBrains.Annotations;

namespace EdiSource.Domain.Tests.Validation.SourceGeneration;

[TestSubject(typeof(BeTimeAttribute))]
public class BeTimeAttributeTest
{
    [Theory]
    [InlineData("1234", true)] // Valid 24-hour time
    [InlineData("2359", true)] // Valid 24-hour time
    [InlineData("0000", true)] // Valid midnight
    [InlineData("2400", false)] // Invalid hour
    [InlineData("9999", false)] // Invalid time
    [InlineData("ABCD", false)] // Invalid format
    public void Validate_WithTimeValue_ShouldValidateCorrectly(string timeValue, bool shouldBeValid)
    {
        // Arrange
        var attribute = new BeTimeAttribute(ValidationSeverity.Critical, 1, 0);
        var segment = new Segment($"TEST*{timeValue}");

        // Act
        var result = attribute.Validate(segment).ToList();

        // Assert
        if (shouldBeValid)
            result.Should().BeEmpty();
        else
            result.Should().ContainSingle()
                .Which.Severity.Should().Be(ValidationSeverity.Critical);
    }
}