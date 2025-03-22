namespace EdiSource.Domain.Tests.Validation.SourceGeneration;

[TestSubject(typeof(ElementLengthAttribute))]
public class ElementLengthAttributeTest
{
    [Theory]
    [InlineData("ABC", 3, 3, true)] // Exact length
    [InlineData("ABCD", 3, 5, true)] // Within range
    [InlineData("AB", 3, 5, false)] // Too short
    [InlineData("ABCDEF", 3, 5, false)] // Too long
    public void Validate_WithLength_ShouldValidateCorrectly(string value, int min, int max, bool shouldBeValid)
    {
        // Arrange
        var attribute = new ElementLengthAttribute(ValidationSeverity.Critical, 1, min, max);
        var segment = new Segment($"TEST*{value}");

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