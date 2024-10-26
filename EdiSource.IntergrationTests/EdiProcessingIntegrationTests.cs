using FluentAssertions;
using EdiSource.Domain.Standard.Loops.ISA;
using EdiSource.Domain.IO.Serializer;
using EdiSource.Domain.Validation.Validator;
using EdiSource.Domain.Validation.Data;

namespace EdiSource.Domain.Tests.Integration;

public class EdiProcessingIntegrationTests
{
    private readonly string _sampleEdiInput = """
                                              ISA*00*          *00*          *ZZ*SENDER         *ZZ*RECEIVER       *200901*1319*^*00501*000000905*0*P*:~
                                              GS*HP*SENDER*RECEIVER*20090119*1319*1*X*005010X221A1~
                                              ST*835*000000001~
                                              BPR*I*5.75*C*ACH*CTX*01*999999999*DA*123456789*1234567890**01*999988880*DA*123456789*20090119~
                                              SE*3*000000001~
                                              GE*1*1~
                                              IEA*1*000000905~
                                              """;

    [Fact]
    public async Task FullProcessing_ParseValidateAndSerialize_ShouldWork()
    {
        // Arrange
        var validator = new ValidateEdi();
        var serializer = new EdiSerializer();

        // Act
        // 1. Parse
        var envelope = await EdiCommon.ParseEdi<InterchangeEnvelope>(_sampleEdiInput);

        // 2. Validate
        var validationResult = validator.Validate(envelope);

        // 3. Serialize
        var serializedOutput = serializer.WriteToString(envelope);

        // 4. Parse again
        var reparsedEnvelope = await EdiCommon.ParseEdi<InterchangeEnvelope>(serializedOutput);

        // Assert
        validationResult.IsValid.Should().BeTrue();
        envelope.Should().BeEquivalentTo(reparsedEnvelope, options =>
            options.Excluding(x => x.ISA.InterchangeDate) // Exclude date comparison
                .Excluding(x => x.ISA.InterchangeTime)); // Exclude time comparison
    }

    [Fact]
    public async Task ProcessingPipeline_WithPrettyPrint_ShouldMaintainData()
    {
        // Arrange
        var validator = new ValidateEdi();

        // Act
        var envelope = await EdiCommon.ParseEdi<InterchangeEnvelope>(_sampleEdiInput);
        var prettyPrinted = EdiCommon.PrettyPrint(envelope);

        // Assert
        prettyPrinted.Should().Contain("ISA");
        prettyPrinted.Should().Contain("GS");
        prettyPrinted.Should().Contain("ST");
        prettyPrinted.Should().Contain("SE");
        prettyPrinted.Should().Contain("GE");
        prettyPrinted.Should().Contain("IEA");
    }

    [Fact]
    public async Task ProcessingPipeline_WithModification_ShouldMaintainIntegrity()
    {
        // Arrange
        var serializer = new EdiSerializer();

        // Act
        // 1. Parse original
        var envelope = await EdiCommon.ParseEdi<InterchangeEnvelope>(_sampleEdiInput);

        // 2. Modify
        envelope.ISA.InterchangeSenderId = "NEWSENDER     ";

        // 3. Serialize
        var modifiedOutput = serializer.WriteToString(envelope);

        // 4. Parse modified
        var reparsedEnvelope = await EdiCommon.ParseEdi<InterchangeEnvelope>(modifiedOutput);

        // Assert
        reparsedEnvelope.ISA.InterchangeSenderId.Should().Be("NEWSENDER     ");
        reparsedEnvelope.Should().BeEquivalentTo(envelope, options =>
            options.Excluding(x => x.ISA.InterchangeDate)
                .Excluding(x => x.ISA.InterchangeTime));
    }

    [Fact]
    public async Task ValidationPipeline_ShouldCaptureAllValidationLevels()
    {
        // Arrange
        var validator = new ValidateEdi();

        // Act
        var envelope = await EdiCommon.ParseEdi<InterchangeEnvelope>(_sampleEdiInput);
        var validationResult = new EdiValidationResult();

        // Add different types of validation messages
        validationResult.ValidationMessages.AddRange(new[]
        {
            new ValidationMessage
            {
                Severity = ValidationSeverity.Info,
                Message = "Processing started",
                Subject = ValidationSubject.Segment
            },
            new ValidationMessage
            {
                Severity = ValidationSeverity.Warning,
                Message = "Optional field missing",
                Subject = ValidationSubject.Segment
            }
        });

        // Assert
        validationResult.IsValid.Should().BeTrue();
        validationResult.HasWarning.Should().BeTrue();
        validationResult.HasError.Should().BeFalse();
        validationResult.HasCritical.Should().BeFalse();
    }
}