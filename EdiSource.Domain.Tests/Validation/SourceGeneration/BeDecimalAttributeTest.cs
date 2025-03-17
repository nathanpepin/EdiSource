namespace EdiSource.Domain.Tests.Validation.SourceGeneration;

[TestSubject(typeof(BeDecimalAttribute))]
public class BeDecimalAttributeTest
{
    [Fact]
    public void Validate_WithValidDecimal_ShouldNotReturnValidationMessage()
    {
        // Arrange
        var attribute = new BeDecimalAttribute(ValidationSeverity.Critical, 1, 0);
        var segment = new Segment("TEST*123.45");

        // Act
        var result = attribute.Validate(segment);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WithInvalidDecimal_ShouldReturnValidationMessage()
    {
        // Arrange
        var attribute = new BeDecimalAttribute(ValidationSeverity.Critical, 1, 0);
        var segment = new Segment("TEST*NOT_DECIMAL");

        // Act
        var result = attribute.Validate(segment);

        // Assert
        result.Should().ContainSingle()
            .Which.Severity.Should().Be(ValidationSeverity.Critical);
    }
}