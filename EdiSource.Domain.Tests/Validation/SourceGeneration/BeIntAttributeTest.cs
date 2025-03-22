namespace EdiSource.Domain.Tests.Validation.SourceGeneration;

[TestSubject(typeof(BeIntAttribute))]
public class BeIntAttributeTest
{
    [Fact]
    public void Validate_WithValidInteger_ShouldNotReturnValidationMessage()
    {
        // Arrange
        var attribute = new BeIntAttribute(ValidationSeverity.Critical, 1, 0);
        var segment = new Segment("TEST*123");

        // Act
        var result = attribute.Validate(segment);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WithInvalidInteger_ShouldReturnValidationMessage()
    {
        // Arrange
        var attribute = new BeIntAttribute(ValidationSeverity.Critical, 1, 0);
        var segment = new Segment("TEST*NOT_INT");

        // Act
        var result = attribute.Validate(segment);

        // Assert
        result.Should().ContainSingle()
            .Which.Severity.Should().Be(ValidationSeverity.Critical);
    }
}