namespace EdiSource.Domain.Tests.Validation.SourceGeneration;

[TestSubject(typeof(EmptyAttribute))]
public class EmptyAttributeTest
{
    [Fact]
    public void Validate_WithEmptyValue_ShouldNotReturnValidationMessage()
    {
        // Arrange
        var attribute = new EmptyAttribute(ValidationSeverity.Critical, 1, 0);
        var segment = new Segment("TEST*");

        // Act
        var result = attribute.Validate(segment);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WithNonEmptyValue_ShouldReturnValidationMessage()
    {
        // Arrange
        var attribute = new EmptyAttribute(ValidationSeverity.Critical, 1, 0);
        var segment = new Segment("TEST*VALUE");

        // Act
        var result = attribute.Validate(segment);

        // Assert
        result.Should().ContainSingle()
            .Which.Severity.Should().Be(ValidationSeverity.Critical);
    }
}