namespace EdiSource.Domain.Tests.IO.Serializer;

[TestSubject(typeof(EdiSerializer))]
public sealed class SerializationTests
{
    // Sample EDI content for testing
    private readonly string _sampleEdiInput =
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
    public async Task RoundTrip_ParseModifySerializeParse_ShouldPreserveStructure()
    {
        // Arrange - Parse original EDI
        var envelope = await EdiCommon.ParseEdi<InterchangeEnvelope>(_sampleEdiInput);

        // Act - Modify and serialize
        envelope.ISA.InterchangeSenderId = "MODIFIED       ";
        var serialized = EdiCommon.WriteEdiToString(envelope);

        // Act - Parse again
        var reparsed = await EdiCommon.ParseEdi<InterchangeEnvelope>(serialized);

        // Assert
        reparsed.ISA.InterchangeSenderId.Should().Be("MODIFIED       ");
        reparsed.FunctionalGroups.Should().HaveCount(1);
        reparsed.FunctionalGroups[0].TransactionSets.Should().HaveCount(1);
    }

    [Fact]
    public async Task RoundTrip_WithStructuralChanges_ShouldPreserveAllChanges()
    {
        // Arrange - Parse original EDI
        var envelope = await EdiCommon.ParseEdi<InterchangeEnvelope>(_sampleEdiInput);

        // Act - Make structural changes
        // Add a new functional group
        var newGroup = new FunctionalGroup
        {
            GS = new GS { E06GroupControlNumber = "2" },
            GE = new GE
            {
                E01NumberOfTransactionSets = 0,
                E02GroupControlNumber = "2"
            },
            Parent = envelope
        };

        envelope.FunctionalGroups.Add(newGroup);

        // Update IEA count
        envelope.IEA.E01NumberOfFunctionalGroups = 2;

        // Serialize
        var serialized = EdiCommon.WriteEdiToString(envelope);

        // Parse again
        var reparsed = await EdiCommon.ParseEdi<InterchangeEnvelope>(serialized);

        // Assert
        reparsed.FunctionalGroups.Should().HaveCount(2);
        reparsed.IEA.E01NumberOfFunctionalGroups.Should().Be(2);
        reparsed.FunctionalGroups[1].GS.E06GroupControlNumber.Should().Be("2");
    }

    [Fact]
    public async Task PrettyPrint_ShouldGenerateReadableFormat()
    {
        // Arrange - Parse EDI
        var envelope = await EdiCommon.ParseEdi<InterchangeEnvelope>(_sampleEdiInput);

        // Act - Generate pretty print
        var prettyPrinted = EdiCommon.PrettyPrint(envelope);

        // Assert
        prettyPrinted.Should().Contain("InterchangeEnvelope");
        prettyPrinted.Should().Contain("ISA*00*");
        prettyPrinted.Should().Contain("FunctionalGroup");
        prettyPrinted.Should().Contain("GS*HP*");
        prettyPrinted.Should().Contain("GenericTransactionSet");
        prettyPrinted.Should().Contain("ST*835*");

        // Verify indentation format
        prettyPrinted.Should().ContainAll(
            "    ISA*",
            "    FunctionalGroup",
            "        GS*"
        );
    }

    [Fact]
    public async Task WriteToString_ShouldGenerateCorrectEdiString()
    {
        // Arrange - Parse EDI
        var envelope = await EdiCommon.ParseEdi<InterchangeEnvelope>(_sampleEdiInput);

        // Act - Serialize to string with both variants
        var serializer = new EdiSerializer();
        var ediString1 = serializer.WriteToString(envelope);
        var ediString2 = EdiCommon.WriteEdiToString(envelope);

        // Assert
        ediString1.Should().Contain("ISA*00*");
        ediString1.Should().Contain("GS*HP*");
        ediString1.Should().Contain("ST*835*");
        ediString1.Should().Contain("SE*3*");
        ediString1.Should().Contain("GE*1*");
        ediString1.Should().Contain("IEA*1*");

        // Both methods should produce equivalent output
        ediString2.Should().BeEquivalentTo(ediString1);
    }

    [Fact]
    public async Task WriteToStream_ShouldWriteCorrectEdiContent()
    {
        // Arrange - Parse EDI and create stream
        var envelope = await EdiCommon.ParseEdi<InterchangeEnvelope>(_sampleEdiInput);
        var stream = new MemoryStream();

        // Act - Serialize to stream
        var serializer = new EdiSerializer();
        await serializer.WriteToStream(envelope, stream);

        // Read back from stream
        stream.Position = 0;
        using var reader = new StreamReader(stream);
        var content = await reader.ReadToEndAsync();

        // Assert
        content.Should().Contain("ISA*00*");
        content.Should().Contain("GS*HP*");
        content.Should().Contain("ST*835*");

        // Verify we can parse the stream content back into an envelope
        var streamEnvelope = await EdiCommon.ParseEdi<InterchangeEnvelope>(content);
        streamEnvelope.ISA.Should().NotBeNull();
        streamEnvelope.FunctionalGroups.Should().HaveCount(1);
    }

    [Fact]
    public async Task WriteToFile_ShouldWriteCorrectEdiFile()
    {
        // Arrange - Parse EDI
        var envelope = await EdiCommon.ParseEdi<InterchangeEnvelope>(_sampleEdiInput);

        // Create temporary file
        var tempFile = Path.GetTempFileName();
        try
        {
            // Act - Serialize to file (using both methods)
            var serializer = new EdiSerializer();
            var fileInfo = new FileInfo(tempFile);
            await serializer.WriteToFile(envelope, fileInfo);

            // Read file content
            var fileContent = await File.ReadAllTextAsync(tempFile);

            // Assert
            fileContent.Should().Contain("ISA*00*");
            fileContent.Should().Contain("GS*HP*");
            fileContent.Should().Contain("ST*835*");

            // Parse from file directly using EdiCommon
            var fileEnvelope = await EdiCommon.ParseEdi<InterchangeEnvelope>(fileInfo);
            fileEnvelope.ISA.Should().NotBeNull();
            fileEnvelope.FunctionalGroups.Should().HaveCount(1);
        }
        finally
        {
            // Clean up
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    [Fact]
    public async Task WriteWithoutNewlines_ShouldGenerateCorrectFormat()
    {
        // Arrange - Parse EDI
        var envelope = await EdiCommon.ParseEdi<InterchangeEnvelope>(_sampleEdiInput);

        // Act - Serialize with and without newlines
        var serializer = new EdiSerializer();
        var withNewlines = serializer.WriteToString(envelope, includeNewLine: true);
        var withoutNewlines = serializer.WriteToString(envelope, includeNewLine: false);

        // Assert
        withNewlines.Should().Contain(Environment.NewLine);
        withoutNewlines.Should().NotContain(Environment.NewLine);

        // Both should be parseable
        var parsedWithNewlines = await EdiCommon.ParseEdi<InterchangeEnvelope>(withNewlines);
        var parsedWithoutNewlines = await EdiCommon.ParseEdi<InterchangeEnvelope>(withoutNewlines);

        parsedWithNewlines.ISA.Should().NotBeNull();
        parsedWithoutNewlines.ISA.Should().NotBeNull();
    }

    [Fact]
    public async Task SerializeWithCustomSeparators_ShouldPreserveSeparators()
    {
        // Arrange - Parse EDI with custom separators
        var customSeparators = new Domain.Separator.Separators('|', '+', '/');
        var customEdi = _sampleEdiInput.Replace('~', '|').Replace('*', '+').Replace(':', '/');

        var envelope = await EdiCommon.ParseEdi<InterchangeEnvelope>(customEdi, customSeparators);

        // Act - Serialize
        var serialized = EdiCommon.WriteEdiToString(envelope, customSeparators);

        // Assert
        serialized.Should().Contain("ISA+00+");
        serialized.Should().Contain("|");
        serialized.Should().NotContain("~");

        // Verify we can parse it back
        var reparsed = await EdiCommon.ParseEdi<InterchangeEnvelope>(serialized, customSeparators);
        reparsed.ISA.Should().NotBeNull();
    }
}