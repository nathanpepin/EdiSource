using EdiSource.Domain.Standard.Segments.DTPData;

namespace EdiSource.Domain.Tests.Validation;

public class EdiValidationTests
{
    private readonly ValidateEdi _validator = new();
    private readonly ValidationMessageCsvConverter _csvConverter = new();

    // Test 1: Standard Segment Validation with Valid Data
    [Fact]
    public void StandardSegment_WithValidData_ShouldPassValidation()
    {
        // Arrange
        var isa = ISA.CreateDefault(
            senderQualifier: "ZZ",
            senderId: "SENDER         ",
            receiverQualifier: "ZZ",
            receiverId: "RECEIVER       ",
            controlNumber: 12345,
            usageIndicator: "P"
        );

        // Act
        var result = _validator.Validate(isa);

        // Assert
        result.IsValid.Should().BeTrue();
        result.ValidationMessages.Should().BeEmpty();
    }

    // Test 2: Standard Segment Validation with Invalid Data
    [Fact]
    public void StandardSegment_WithInvalidData_ShouldFailValidation()
    {
        // Arrange
        var isa = ISA.CreateDefault(
            senderQualifier: "ZZ",
            senderId: "SENDER         ",
            receiverQualifier: "ZZ",
            receiverId: "RECEIVER       ",
            controlNumber: 12345,
            usageIndicator: "P"
        );

        // Invalid sender qualifier (making it too long)
        isa.AuthorizationInformationQualifier = "000"; // should be 2 characters

        // Act
        var result = _validator.Validate(isa);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ValidationMessages.Should().NotBeEmpty();
        result.ValidationMessages.Should().Contain(m =>
            m.Severity >= ValidationSeverity.Error &&
            m.DataElement == 1 &&
            m.Message.Contains("length"));
    }

    // Test 3: Custom Validation Rules
    [Fact]
    public void CustomValidationRule_ShouldBeApplied()
    {
        // Arrange
        var segment = new REF
        {
            E01Identifier = "REF",
            // Both E02Identification and E03Description are null, which should trigger validation error
        };

        // Act
        var result = _validator.Validate(segment);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ValidationMessages.Should().NotBeEmpty();
        result.ValidationMessages.Should().Contain(m =>
            m.Severity >= ValidationSeverity.Error &&
            m.Message.Contains("REF required either 2 or 3 elements"));
    }

    // Test 4: Validation Reporting and Severity Levels
    [Fact]
    public void ValidationResult_WithDifferentSeverityLevels_ShouldReportCorrectly()
    {
        // Arrange
        var result = new EdiValidationResult();
        result.ValidationMessages.AddRange(new[]
        {
            new ValidationMessage
            {
                Severity = ValidationSeverity.Info,
                Message = "Info message",
                Subject = ValidationSubject.Segment
            },
            new ValidationMessage
            {
                Severity = ValidationSeverity.Warning,
                Message = "Warning message",
                Subject = ValidationSubject.Segment
            },
            new ValidationMessage
            {
                Severity = ValidationSeverity.Error,
                Message = "Error message",
                Subject = ValidationSubject.Segment
            },
            new ValidationMessage
            {
                Severity = ValidationSeverity.Critical,
                Message = "Critical message",
                Subject = ValidationSubject.Segment
            }
        });

        // Act & Assert
        result.IsValid.Should().BeFalse();
        result.HasWarning.Should().BeTrue();
        result.HasError.Should().BeTrue();
        result.HasCritical.Should().BeTrue();

        result.WhereIsValid.Should().HaveCount(1);
        result.WhereHasWarning.Should().HaveCount(3);
        result.WhereHasError.Should().HaveCount(2);
        result.WhereHasCritical.Should().HaveCount(1);
    }

    // Test 5: Validation Message CSV Output
    [Fact]
    public async Task ValidationMessageCsv_ShouldContainAllMessageData()
    {
        // Arrange
        var ediValidationResult = new EdiValidationResult();
        var tempFilePath = Path.GetTempFileName();
        var fileInfo = new FileInfo(tempFilePath);

        // Add validation messages with different severity levels
        ediValidationResult.AddRange(new[]
        {
            new ValidationMessage
            {
                Severity = ValidationSeverity.Critical,
                Message = "Missing required field",
                Subject = ValidationSubject.Segment,
                SegmentLine = 1,
                Segment = "ST*835*0001",
                DataElement = 3
            },
            new ValidationMessage
            {
                Severity = ValidationSeverity.Error,
                Message = "Invalid qualifier",
                Subject = ValidationSubject.Segment,
                SegmentLine = 2,
                Segment = "REF*AA*123",
                DataElement = 1,
                Value = "AA"
            },
            new ValidationMessage
            {
                Severity = ValidationSeverity.Warning,
                Message = "Date format warning",
                Subject = ValidationSubject.Segment,
                SegmentLine = 3,
                Segment = "DTM*472*20231010",
                DataElement = 2,
                Value = "20231010"
            },
            new ValidationMessage
            {
                Severity = ValidationSeverity.Info,
                Message = "Informational message",
                Subject = ValidationSubject.Loop,
                LoopLine = 1,
                Loop = "TransactionSet"
            }
        });

        try
        {
            // Act
            var csvString = _csvConverter.ToCsvString(ediValidationResult);
            await _csvConverter.WriteToCsvAsync(ediValidationResult, fileInfo);

            // Assert - String output
            csvString.Should().NotBeNullOrEmpty();
            csvString.Should().StartWith("ValidationSeverity,Message,Subject");
            csvString.Should().Contain("Critical,Missing required field,Segment");
            csvString.Should().Contain("Error,Invalid qualifier,Segment");
            csvString.Should().Contain("Warning,Date format warning,Segment");
            csvString.Should().Contain("Info,Informational message,Loop");

            // Assert - File output
            File.Exists(tempFilePath).Should().BeTrue();
            var fileContent = await File.ReadAllTextAsync(tempFilePath);
            fileContent.Should().Be(csvString);
        }
        finally
        {
            // Cleanup
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }
    }

    // Test 6: Source-Generated Validation Rules
    [Fact]
    public void SourceGeneratedValidationRules_ShouldBeApplied()
    {
        // Arrange - Create a segment with source-generated validation rules
        var dtp = new DTP
        {
            
            Qualifier = "356",
            DateFormatCode = DateFormatCode.D8,
        };

        // Set an element directly to create invalid state for validation
        dtp.SetCompositeElement("NOT-A-DATE", 3);

        // Act
        var result = _validator.Validate(dtp);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ValidationMessages.Should().NotBeEmpty();
        // Validation should catch the invalid date format
        result.ValidationMessages.Should().Contain(m =>
            m.Severity >= ValidationSeverity.Error &&
            m.DataElement == 3 &&
            m.Message.Contains("date"));
    }

    // Test 7: Complex Validation Across Multiple Segments
    [Fact]
    public void ValidateComplexStructure_ShouldProcessAllSegments()
    {
        // Arrange - Create a mini envelope with multiple segments
        var envelope = new InterchangeEnvelope
        {
            ISA = ISA.CreateDefault(
                senderQualifier: "ZZ",
                senderId: "SENDER         ",
                receiverQualifier: "ZZ",
                receiverId: "RECEIVER       ",
                controlNumber: 12345,
                usageIndicator: "X" // Invalid value - should be P or T
            ),
            IEA = new IEA
            {
                E01NumberOfFunctionalGroups = 0, // Incorrect count
                E02InterchangeControlNumber = "000012345"
            }
        };

        // Act
        var result = _validator.Validate(envelope);

        // Assert
        result.IsValid.Should().BeFalse();
        result.ValidationMessages.Should().HaveCountGreaterThan(1);
        // Should find issues in both ISA and IEA segments
        result.ValidationMessages.Should().Contain(m => m.Message.Contains("Usage Indicator") ||
                                                        m.Message.Contains("NumberOfFunctionalGroups"));
    }
}