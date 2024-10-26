using EdiSource.Domain.Segments;
using EdiSource.Domain.Separator;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.Factory;
using EdiSource.Domain.Validation.Validator;
using FluentAssertions;

namespace EdiSource.Domain.Tests.Validation.Validator;

public class ValidationTests
{
    private readonly Separators _separators = new();
    private readonly ValidateEdi _validator = new();

    [Fact]
    public void Validate_WithValidSegment_ShouldReturnNoErrors()
    {
        // Arrange
        var segment = new Segment("ST*835*000000001", _separators);

        // Act
        var result = _validator.Validate(segment);

        // Assert
        result.IsValid.Should().BeTrue();
        result.ValidationMessages.Should().BeEmpty();
    }

    [Fact]
    public void ValidationResult_WithCriticalError_ShouldReportCorrectSeverity()
    {
        // Arrange
        var result = new EdiValidationResult();
        var criticalMessage = new ValidationMessage
        {
            Severity = ValidationSeverity.Critical,
            Message = "Critical error",
            Subject = ValidationSubject.Segment
        };

        // Act
        result.ValidationMessages.Add(criticalMessage);

        // Assert
        result.HasCritical.Should().BeTrue();
        result.WhereHasCritical.Should().ContainSingle();
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void ValidationResult_WithMultipleSeverities_ShouldFilterCorrectly()
    {
        // Arrange
        var result = new EdiValidationResult();
        result.ValidationMessages.AddRange(new[]
        {
            new ValidationMessage { Severity = ValidationSeverity.Info, Message = "Info" },
            new ValidationMessage { Severity = ValidationSeverity.Warning, Message = "Warning" },
            new ValidationMessage { Severity = ValidationSeverity.Error, Message = "Error" },
            new ValidationMessage { Severity = ValidationSeverity.Critical, Message = "Critical" }
        });

        // Assert
        result.WhereIsValid.Should().HaveCount(1);
        result.WhereHasWarning.Should().HaveCount(3);
        result.WhereHasError.Should().HaveCount(2);
        result.WhereHasCritical.Should().HaveCount(1);
    }

    [Fact]
    public void ValidationFactory_ShouldCreateCorrectMessage()
    {
        // Arrange
        var segment = new Segment("ST*835*000000001", _separators);

        // Act
        var message = ValidationFactory.Create(segment, ValidationSeverity.Error, "Test error", 1, 0);

        // Assert
        message.Severity.Should().Be(ValidationSeverity.Error);
        message.Message.Should().Be("Test error");
        message.Subject.Should().Be(ValidationSubject.Segment);
        message.DataElement.Should().Be(1);
        message.CompositeElement.Should().Be(0);
        message.Value.Should().Be("835");
    }

    [Theory]
    [InlineData(ValidationSeverity.Info)]
    [InlineData(ValidationSeverity.Warning)]
    [InlineData(ValidationSeverity.Error)]
    [InlineData(ValidationSeverity.Critical)]
    public void ValidationMessage_ToString_ShouldIncludeAllRelevantInformation(ValidationSeverity severity)
    {
        // Arrange
        var message = new ValidationMessage
        {
            Severity = severity,
            Message = "Test message",
            Subject = ValidationSubject.Segment,
            SegmentLine = 1,
            DataElement = 2,
            Value = "TEST"
        };

        // Act
        var result = message.ToString();

        // Assert
        result.Should().Contain(severity.ToString());
        result.Should().Contain("Test message");
        result.Should().Contain("Segment");
        result.Should().Contain("SegmentLine: 1");
        result.Should().Contain("DataElement: 2");
        result.Should().Contain("Value: TEST");
    }
}