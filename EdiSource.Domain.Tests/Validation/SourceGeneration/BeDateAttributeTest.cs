namespace EdiSource.Domain.Tests.Validation.SourceGeneration;

[TestSubject(typeof(BeDateAttribute))]
public class BeDateAttributeTest
{
    [Fact]
    public void Validate_WithValidDate_ShouldNotReturnValidationMessage()
    {
        // Arrange
        var attribute = new BeDateAttribute(ValidationSeverity.Critical, 1, 0);
        var segment = new Segment("TEST*20240101");

        // Act
        var result = attribute.Validate(segment);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WithInvalidDate_ShouldReturnValidationMessage()
    {
        // Arrange
        var attribute = new BeDateAttribute(ValidationSeverity.Critical, 1, 0);
        var segment = new Segment("TEST*INVALID");

        // Act
        var result = attribute.Validate(segment);

        // Assert
        result.Should().ContainSingle()
            .Which.Severity.Should().Be(ValidationSeverity.Critical);
    }
}