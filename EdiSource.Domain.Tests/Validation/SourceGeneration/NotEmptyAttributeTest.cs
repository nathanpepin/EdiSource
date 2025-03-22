namespace EdiSource.Domain.Tests.Validation.SourceGeneration;

[TestSubject(typeof(NotEmptyAttribute))]
public class NotEmptyAttributeTest
{
    [Fact]
    public void Validate_WithNonEmptyValue_ShouldNotReturnValidationMessage()
    {
        // Arrange
        var attribute = new NotEmptyAttribute(ValidationSeverity.Critical, 1, 0);
        var segment = new Segment("TEST*VALUE");

        // Act
        var result = attribute.Validate(segment);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WithEmptyValue_ShouldReturnValidationMessage()
    {
        // Arrange
        var attribute = new NotEmptyAttribute(ValidationSeverity.Critical, 1, 0);
        var segment = new Segment("TEST*");

        // Act
        var result = attribute.Validate(segment);

        // Assert
        result.Should().ContainSingle()
            .Which.Severity.Should().Be(ValidationSeverity.Critical);
    }
}