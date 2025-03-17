namespace EdiSource.Domain.Tests.Validation.SourceGeneration;

[TestSubject(typeof(BeDateTimeAttribute))]
public class BeDateTimeAttributeTest
{
    [Fact]
    public void Validate_WithValidDateTime_ShouldNotReturnValidationMessage()
    {
        // Arrange
        var attribute = new BeDateTimeAttribute(ValidationSeverity.Critical, 1, 0);
        var segment = new Segment("TEST*202401011200");

        // Act
        var result = attribute.Validate(segment);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WithInvalidDateTime_ShouldReturnValidationMessage()
    {
        // Arrange
        var attribute = new BeDateTimeAttribute(ValidationSeverity.Critical, 1, 0);
        var segment = new Segment("TEST*INVALID");

        // Act
        var result = attribute.Validate(segment);

        // Assert
        result.Should().ContainSingle()
            .Which.Severity.Should().Be(ValidationSeverity.Critical);
    }
}