namespace EdiSource.Domain.Tests.Validation.SourceGeneration;

[TestSubject(typeof(RequiredElementAttribute))]
public class RequiredElementAttributeTest
{
    [Fact]
    public void Validate_WithAllRequiredElements_ShouldNotReturnValidationMessage()
    {
        // Arrange
        var attribute = new RequiredDataElementsAttribute(ValidationSeverity.Critical, new[] { 0, 1 });
        var segment = new Segment("TEST*VALUE");

        // Act
        var result = attribute.Validate(segment);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WithMissingRequiredElement_ShouldReturnValidationMessage()
    {
        // Arrange
        var attribute = new RequiredDataElementsAttribute(ValidationSeverity.Critical, new[] { 0, 1, 2 });
        var segment = new Segment("TEST*VALUE");

        // Act
        var result = attribute.Validate(segment);

        // Assert
        result.Should().ContainSingle()
            .Which.Severity.Should().Be(ValidationSeverity.Critical);
    }
}