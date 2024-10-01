namespace EdiSource.Domain.Tests.IO.Parser;

[TestSubject(typeof(EdiParser<>))]
public class EdiParserTests
{
    private readonly string _validEdiInput =
        """
        ISA*00*          *00*          *ZZ*SENDER         *ZZ*RECEIVER       *200901*1319*^*00501*000000905*0*P*:~
        GS*HP*SENDER*RECEIVER*20090119*1319*1*X*005010X221A1~
        ST*835*000000001~
        BPR*I*5.75*C*ACH*CTX*01*999999999*DA*123456789*1234567890**01*999988880*DA*123456789*20090119~
        SE*3*000000001~
        GE*1*1~
        IEA*1*000000905~
        """;

    [Fact]
    public async Task ParseEdi_WithValidInput_ShouldParseSuccessfully()
    {
        // Arrange
        var parser = new EdiParser<InterchangeEnvelope>();

        // Act
        var result = await parser.ParseEdi(_validEdiInput);

        // Assert
        result.Should().NotBeNull();
        result.ISA.Should().NotBeNull();
        result.FunctionalGroups.Should().HaveCount(1);
        result.IEA.Should().NotBeNull();
    }

    [Fact]
    public async Task ParseEdi_ShouldParseISASegmentCorrectly()
    {
        // Arrange
        var parser = new EdiParser<InterchangeEnvelope>();

        // Act
        var result = await parser.ParseEdi(_validEdiInput);

        // Assert
        result.ISA.AuthorizationInformationQualifier.Should().Be("00");
        result.ISA.InterchangeSenderId.Should().Be("SENDER         ");
        result.ISA.InterchangeReceiverId.Should().Be("RECEIVER       ");
        result.ISA.InterchangeControlVersionNumber.Should().Be("00501");
    }

    [Fact]
    public async Task ParseEdi_FromStream_ShouldParseSuccessfully()
    {
        // Arrange
        var parser = new EdiParser<InterchangeEnvelope>();
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(_validEdiInput));
        using var reader = new StreamReader(stream);

        // Act
        var result = await parser.ParseEdi(reader);

        // Assert
        result.Should().NotBeNull();
        result.ISA.Should().NotBeNull();
    }

    [Fact]
    public async Task ParseEdi_WithInvalidInput_ShouldThrowException()
    {
        // Arrange
        var parser = new EdiParser<InterchangeEnvelope>();
        var invalidInput = "Invalid EDI content";

        // Act & Assert
        await Assert.ThrowsAsync<InvalidISAException>(() => parser.ParseEdi(invalidInput));
    }

    [Fact]
    public async Task ParseEdi_WithCustomSeparators_ShouldParseCorrectly()
    {
        // Arrange
        var parser = new EdiParser<InterchangeEnvelope>();
        var customSeparators = new Separators('|', '+', '/');
        var customInput = _validEdiInput.Replace('~', '|').Replace('*', '+').Replace(':', '/');

        // Act
        var result = await parser.ParseEdi(customInput, customSeparators);

        // Assert
        result.Should().NotBeNull();
        result.ISA.Should().NotBeNull();
    }
}