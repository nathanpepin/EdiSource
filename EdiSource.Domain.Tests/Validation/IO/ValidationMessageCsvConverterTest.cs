using EdiSource.Domain.Standard.Loops.ISA;
using EdiSource.Domain.Validation.Factory;
using EdiSource.Domain.Validation.IO;
using EdiSource.Domain.Validation.Validator;

namespace EdiSource.Domain.Tests.Validation.IO;

[TestSubject(typeof(ValidationMessageCsvConverter))]
public class ValidationMessageCsvConverterTest
{
    private readonly ValidationMessageCsvConverter _sut = new();

    [Fact]
    public Task ValidationMessageCsvConverter_ToCsvString_Should_Return_Expected_Result()
    {
        //Arrange
        var ediValidationResult = new EdiValidationResult();
        var loop = new InterchangeEnvelope();

        ediValidationResult.AddRange([
            ValidationFactory.CreateCritical(loop, "Critical"),
            ValidationFactory.CreateError(loop, "Error"),
            ValidationFactory.CreateWarning(loop, "Warning"),
            ValidationFactory.CreateInfo(loop, "Info")
        ]);

        //Act
        var result = _sut.ToCsvString(ediValidationResult);

        //Assert
        return Verify(result);
    }

    [Fact]
    public Task ValidationMessageCsvConverter_ToCsvString_Should_Return_No_Result_When_Empty()
    {
        //Arrange
        var ediValidationResult = new EdiValidationResult();

        //Act
        var result = _sut.ToCsvString(ediValidationResult);

        //Assert
        return Verify(result);
    }
}