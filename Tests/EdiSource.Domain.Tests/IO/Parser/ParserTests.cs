namespace EdiSource.Domain.Tests.IO.Parser;

/// <summary>
///     Tests for the EDI parser functionality focusing on different transaction sets,
///     custom separators, error handling, and async operations.
/// </summary>
public sealed class ParserTests
{
    #region Test Data

    // Common ISA/GS envelope for test data
    private const string EnvelopePrefix =
        "ISA*00*          *00*          *ZZ*SENDER         *ZZ*RECEIVER       *230501*1200*^*00501*000000001*0*P*:~\n" +
        "GS*HP*SENDER*RECEIVER*20230501*1200*1*X*005010X220A1~\n";

    private const string EnvelopeSuffix =
        "GE*1*1~\n" +
        "IEA*1*000000001~";

    // 834 Benefit Enrollment transaction set
    private const string Transaction834 =
        "ST*834*0001~\n" +
        "BGN*00*12345*20230501*1200~\n" +
        "N1*P5*PAYER NAME*FI*123456789~\n" +
        "N1*IN*EMPLOYER NAME*FI*987654321~\n" +
        "INS*Y*18*030*XN*A*E**FT~\n" +
        "REF*0F*12345~\n" +
        "DTP*356*D8*20230601~\n" +
        "NM1*IL*1*DOE*JOHN*A***34*123456789~\n" +
        "SE*9*0001~\n";

    // 835 Health Care Claim Payment transaction set
    private const string Transaction835 =
        "ST*835*0001~\n" +
        "BPR*I*500.00*C*ACH*CCP*01*999999999*DA*123456789*1234567890**01*999988880*DA*123456789*20230515~\n" +
        "TRN*1*12345*1999999999~\n" +
        "REF*EV*PAYMENTS~\n" +
        "DTM*405*20230515~\n" +
        "N1*PR*PAYER NAME*XV*123456789~\n" +
        "N1*PE*PAYEE NAME*FI*987654321~\n" +
        "SE*8*0001~\n";

    // 837 Health Care Claim transaction set
    private const string Transaction837 =
        "ST*837*0001~\n" +
        "BHT*0019*00*123456*20230501*1200*CH~\n" +
        "NM1*41*2*SUBMITTER NAME*****46*123456789~\n" +
        "PER*IC*CONTACT NAME*TE*5555555555~\n" +
        "NM1*40*2*RECEIVER NAME*****46*987654321~\n" +
        "HL*1**20*1~\n" +
        "NM1*85*2*BILLING PROVIDER*****XX*1234567890~\n" +
        "SE*8*0001~\n";

    // Complete EDI messages with envelope
    private const string Edi834 = EnvelopePrefix + Transaction834 + EnvelopeSuffix;
    private const string Edi835 = EnvelopePrefix + Transaction835 + EnvelopeSuffix;
    private const string Edi837 = EnvelopePrefix + Transaction837 + EnvelopeSuffix;

    // Malformed EDI examples
    private const string MalformedEdiInvalidSegment =
        "ISA*00*          *00*          *ZZ*SENDER         *ZZ*RECEIVER       *230501*1200*^*00501*000000001*0*P*:~\n" +
        "GS*HP*SENDER*RECEIVER*20230501*1200*1*X*005010X220A1~\n" +
        "ST*834*0001~\n" +
        "INVALIDTAG*00*12345*20230501*1200~\n" + // Invalid segment
        "SE*2*0001~\n" +
        "GE*1*1~\n" +
        "IEA*1*000000001~";

    private const string MalformedEdiMissingSegment =
        "ISA*00*          *00*          *ZZ*SENDER         *ZZ*RECEIVER       *230501*1200*^*00501*000000001*0*P*:~\n" +
        "GS*HP*SENDER*RECEIVER*20230501*1200*1*X*005010X220A1~\n" +
        "ST*834*0001~\n" +
        // Missing SE segment
        "GE*1*1~\n" +
        "IEA*1*000000001~";

    // EDI with custom separators
    private const string EdiWithCustomSeparators =
        "ISA|00|          |00|          |ZZ|SENDER         |ZZ|RECEIVER       |230501|1200|^|00501|000000001|0|P|/]\n" +
        "GS|HP|SENDER|RECEIVER|20230501|1200|1|X|005010X220A1]\n" +
        "ST|834|0001]\n" +
        "BGN|00|12345|20230501|1200]\n" +
        "SE|3|0001]\n" +
        "GE|1|1]\n" +
        "IEA|1|000000001]";

    #endregion

    #region Transaction Set Parsing Tests

    [Fact]
    public async Task ParseEdi_834BenefitEnrollment_ShouldParseSuccessfully()
    {
        // Arrange
        var parser = new EdiParser<InterchangeEnvelope>();

        // Act
        var result = await parser.ParseEdi(Edi834);

        // Assert
        result.Should().NotBeNull();
        result.ISA.Should().NotBeNull();
        result.FunctionalGroups.Should().HaveCount(1);

        var transactionSets = result.FunctionalGroups[0].TransactionSets;
        transactionSets.Should().HaveCount(1);

        // Verify transaction set type by checking ST segment
        var firstSegment = transactionSets[0].YieldChildSegments().First();
        firstSegment.Should().BeAssignableTo<Segment>();
        firstSegment.GetCompositeElement(0).Should().Be("ST");
        firstSegment.GetCompositeElement(1).Should().Be("834");
    }

    [Fact]
    public async Task ParseEdi_835HealthCareClaimPayment_ShouldParseSuccessfully()
    {
        // Arrange
        var parser = new EdiParser<InterchangeEnvelope>();

        // Act
        var result = await parser.ParseEdi(Edi835);

        // Assert
        result.Should().NotBeNull();
        result.ISA.Should().NotBeNull();
        result.FunctionalGroups.Should().HaveCount(1);

        var transactionSets = result.FunctionalGroups[0].TransactionSets;
        transactionSets.Should().HaveCount(1);

        // Verify transaction set type by checking ST segment
        var firstSegment = transactionSets[0].YieldChildSegments().First();
        firstSegment.Should().BeAssignableTo<Segment>();
        firstSegment.GetCompositeElement(0).Should().Be("ST");
        firstSegment.GetCompositeElement(1).Should().Be("835");
    }

    [Fact]
    public async Task ParseEdi_837HealthCareClaim_ShouldParseSuccessfully()
    {
        // Arrange
        var parser = new EdiParser<InterchangeEnvelope>();

        // Act
        var result = await parser.ParseEdi(Edi837);

        // Assert
        result.Should().NotBeNull();
        result.ISA.Should().NotBeNull();
        result.FunctionalGroups.Should().HaveCount(1);

        var transactionSets = result.FunctionalGroups[0].TransactionSets;
        transactionSets.Should().HaveCount(1);

        // Verify transaction set type by checking ST segment
        var firstSegment = transactionSets[0].YieldChildSegments().First();
        firstSegment.Should().BeAssignableTo<Segment>();
        firstSegment.GetCompositeElement(0).Should().Be("ST");
        firstSegment.GetCompositeElement(1).Should().Be("837");
    }

    [Fact]
    public async Task ParseEdi_MultipleTransactionSets_ShouldParseAllSuccessfully()
    {
        // Arrange
        var multipleTransactions =
            EnvelopePrefix +
            Transaction834 +
            Transaction835 +
            EnvelopeSuffix;

        var parser = new EdiParser<InterchangeEnvelope>();

        // Act
        var result = await parser.ParseEdi(multipleTransactions);

        // Assert
        result.Should().NotBeNull();
        result.FunctionalGroups.Should().HaveCount(1);

        var transactionSets = result.FunctionalGroups[0].TransactionSets;
        transactionSets.Should().HaveCount(2);

        // Verify first transaction set is 834
        var firstTransactionSegments = transactionSets[0].YieldChildSegments().ToList();
        firstTransactionSegments[0].GetCompositeElement(1).Should().Be("834");

        // Verify second transaction set is 835
        var secondTransactionSegments = transactionSets[1].YieldChildSegments().ToList();
        secondTransactionSegments[0].GetCompositeElement(1).Should().Be("835");
    }

    #endregion

    #region Custom Separator Tests

    [Fact]
    public async Task ParseEdi_WithCustomSeparators_ShouldParseSuccessfully()
    {
        // Arrange
        var parser = new EdiParser<InterchangeEnvelope>();
        var customSeparators = new Separators(']', '|', '/');

        // Act
        var result = await parser.ParseEdi(EdiWithCustomSeparators, customSeparators);

        // Assert
        result.Should().NotBeNull();
        result.ISA.Should().NotBeNull();
        result.FunctionalGroups.Should().HaveCount(1);

        var transactionSet = result.FunctionalGroups[0].TransactionSets[0];
        transactionSet.Should().NotBeNull();

        // Verify content parsed with custom separators
        var segments = transactionSet.YieldChildSegments().ToList();
        segments.Should().HaveCount(3); // ST, BGN, SE
        segments[0].GetCompositeElement(0).Should().Be("ST");
        segments[1].GetCompositeElement(0).Should().Be("BGN");
    }

    [Fact]
    public async Task ParseEdi_WithCustomSeparatorsDetection_ShouldAutomaticallyDetectAndUseCorrectSeparators()
    {
        // Arrange
        var parser = new EdiParser<InterchangeEnvelope>();

        // Act
        // Note: Not passing custom separators, allowing auto-detection from ISA segment
        var result = await parser.ParseEdi(EdiWithCustomSeparators);

        // Assert
        result.Should().NotBeNull();
        result.ISA.Should().NotBeNull();
        result.FunctionalGroups.Should().HaveCount(1);

        // Verify ISA segment was interpreted correctly
        result.ISA.InterchangeSenderId.Should().Be("SENDER         ");
        result.ISA.InterchangeReceiverId.Should().Be("RECEIVER       ");
    }

    [Fact]
    public async Task ParseEdi_WithCreateFromISA_ShouldProperlyDetectSeparatorsFromISA()
    {
        // Arrange
        using var streamReader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(EdiWithCustomSeparators)));

        // Act
        var separators = await Separators.CreateFromISA(streamReader);

        // Assert
        separators.Should().NotBeNull();
        separators.SegmentSeparator.Should().Be(']');
        separators.DataElementSeparator.Should().Be('|');
        separators.CompositeElementSeparator.Should().Be('/');
    }

    #endregion

    #region Error Handling Tests

    [Fact]
    public async Task ParseEdi_WithInvalidSegment_ShouldStillParseEnvelopeStructure()
    {
        // Arrange
        var parser = new EdiParser<InterchangeEnvelope>();

        // Act
        var result = await parser.ParseEdi(MalformedEdiInvalidSegment);

        // Assert
        result.Should().NotBeNull();
        result.ISA.Should().NotBeNull();
        result.FunctionalGroups.Should().HaveCount(1);

        // The parser should still be able to parse the structure even with an invalid segment
        var transactionSet = result.FunctionalGroups[0].TransactionSets[0];
        transactionSet.Should().NotBeNull();

        // We should find the invalid segment as a generic segment
        var segments = transactionSet.YieldChildSegments().ToList();
        segments.Should().HaveCountGreaterThan(1);
        segments.Any(s => s.GetCompositeElement(0) == "INVALIDTAG").Should().BeTrue();
    }

    [Fact]
    public async Task ParseEdi_WithCompletelyInvalidInput_ShouldThrowException()
    {
        // Arrange
        var parser = new EdiParser<InterchangeEnvelope>();
        var invalidInput = "This is not a valid EDI document";

        // Act & Assert
        await Assert.ThrowsAsync<InvalidISAException>(() => parser.ParseEdi(invalidInput));
    }

    #endregion

    #region Async and Cancellation Tests

    [Fact]
    public async Task ParseEdi_WithCancellationToken_ShouldRespectCancellation()
    {
        // Arrange
        var parser = new EdiParser<InterchangeEnvelope>();
        var cancellationTokenSource = new CancellationTokenSource();

        // Immediately cancel the token
        await cancellationTokenSource.CancelAsync();

        // Act & Assert
        await Assert.ThrowsAsync<ChannelClosedException>(() =>
            parser.ParseEdi(Edi834, cancellationToken: cancellationTokenSource.Token));
    }

    [Fact]
    public async Task ParseEdi_FromStream_ShouldParseAsynchronously()
    {
        // Arrange
        var parser = new EdiParser<InterchangeEnvelope>();
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(Edi834));
        using var reader = new StreamReader(stream);

        // Act
        var result = await parser.ParseEdi(reader);

        // Assert
        result.Should().NotBeNull();
        result.ISA.Should().NotBeNull();
        result.FunctionalGroups.Should().HaveCount(1);

        var transactionSet = result.FunctionalGroups[0].TransactionSets[0];
        transactionSet.Should().NotBeNull();
    }

    [Fact]
    public async Task ParseEdi_FromFileInfo_ShouldParseAsynchronously()
    {
        // Arrange
        var parser = new EdiParser<InterchangeEnvelope>();
        var tempFile = Path.GetTempFileName();
        await File.WriteAllTextAsync(tempFile, Edi834);
        var fileInfo = new FileInfo(tempFile);

        try
        {
            // Act
            var result = await parser.ParseEdi(fileInfo);

            // Assert
            result.Should().NotBeNull();
            result.ISA.Should().NotBeNull();
            result.FunctionalGroups.Should().HaveCount(1);

            var transactionSet = result.FunctionalGroups[0].TransactionSets[0];
            transactionSet.Should().NotBeNull();
        }
        finally
        {
            // Clean up temp file
            File.Delete(tempFile);
        }
    }

    [Fact]
    public async Task ParseEdiWithCompetingCancellationToken_ShouldCompleteBefore()
    {
        // Arrange
        var parser = new EdiParser<InterchangeEnvelope>();
        var cancellationTokenSource = new CancellationTokenSource();

        // Set token to cancel after 5 seconds (which should be plenty of time for parsing)
        cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(5));

        // Act
        var result = await parser.ParseEdi(Edi834, cancellationToken: cancellationTokenSource.Token);

        // Assert
        result.Should().NotBeNull();
        result.ISA.Should().NotBeNull();
    }

    #endregion
}