namespace EdiSource.Domain.Tests.Validation.SourceGeneration;

[TestSubject(typeof(IsOneOfValuesAttribute))]
public class IsOneOfValuesAttributeTest
{
    [Theory]
    [InlineData("A", true)]
    [InlineData("B", true)]
    [InlineData("C", true)]
    [InlineData("D", false)]
    [InlineData("", false)]
    public void Validate_WithAllowedValues_ShouldValidateCorrectly(string value, bool shouldBeValid)
    {
        // Arrange
        var attribute = new IsOneOfValuesAttribute(ValidationSeverity.Critical, 1, 0, "A", "B", "C");
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