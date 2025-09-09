namespace EdiSource.IntergrationTests;

public class Loop834Tests
{
    private readonly string _sampleEdi =
        """
        ST*834*0001~
        REF*38*123456789~
        INS*Y*18*030*XN*A*E**FT~
        REF*0F*12345~
        DTP*356*D8*20240115~
        NM1*IL*1*DOE*JOHN*A***34*123456789~
        DMG*D8*19800101*M~
        N3*123 MAIN ST*APT 4B~
        N4*ANYTOWN*NY*12345~
        PER*IP*JANE DOE*TE*5555555555~
        SE*11*0001~
        """;

    [Fact]
    public async Task Should_Parse_834_Transaction_Set()
    {
        // Arrange & Act
        var transactionSet = await EdiCommon.ParseEdi<Loop834>(_sampleEdi);

        // Assert
        transactionSet.Should().NotBeNull();
        transactionSet.ST.Should().NotBeNull();
        transactionSet.REFs.Should().NotBeEmpty();
        transactionSet.InsuredLoop.Should().NotBeNull();
        transactionSet.MemberLoops.Should().NotBeEmpty();
        transactionSet.SE.Should().NotBeNull();
    }

    [Fact]
    public async Task Should_Parse_ST_Segment_Correctly()
    {
        // Arrange & Act
        var transactionSet = await EdiCommon.ParseEdi<Loop834>(_sampleEdi);

        // Assert
        transactionSet.ST.TransactionSetControlNumber.Should().Be("0001");
    }

    [Fact]
    public async Task Should_Parse_Loop2000_Correctly()
    {
        // Arrange & Act  
        var transactionSet = await EdiCommon.ParseEdi<Loop834>(_sampleEdi);
        var insuredLoop = transactionSet.InsuredLoop;

        // Assert
        insuredLoop.INS.InsuredIndicator.Should().Be("Y");
        insuredLoop.INS.IndividualRelationshipCode.Should().Be("18");
        insuredLoop.REFs.Should().NotBeEmpty();
        insuredLoop.DTPs.Should().NotBeEmpty();

        var subscriberRef = insuredLoop.REFs.First();
        subscriberRef.ReferenceQualifier.Should().Be("0F");
        subscriberRef.ReferenceId.Should().Be("12345");

        var coverageDate = insuredLoop.DTPs.First();
        coverageDate.DateQualifier.Should().Be("356");
        coverageDate.DateFormatQualifier.Should().Be("D8");
        coverageDate.Date.Should().Be(new DateTime(2024, 1, 15));
    }

    [Fact]
    public async Task Should_Parse_Loop2100_Correctly()
    {
        // Arrange & Act
        var transactionSet = await EdiCommon.ParseEdi<Loop834>(_sampleEdi);
        var memberLoop = transactionSet.MemberLoops.First();

        // Assert
        memberLoop.NM1.EntityIdentifierCode.Should().Be("IL");
        memberLoop.NM1.EntityTypeQualifier.Should().Be("1");
        memberLoop.NM1.LastName.Should().Be("DOE");
        memberLoop.NM1.FirstName.Should().Be("JOHN");

        var demographics = memberLoop.Demographics.First();
        demographics.DateOfBirth.Should().Be(new DateTime(1980, 1, 1));
        demographics.Gender.Should().Be("M");

        var address = memberLoop.Addresses.First();
        address.AddressLine1.Should().Be("123 MAIN ST");
        address.AddressLine2.Should().Be("APT 4B");

        var cityStateZip = memberLoop.CityStateZips.First();
        cityStateZip.City.Should().Be("ANYTOWN");
        cityStateZip.State.Should().Be("NY");
        cityStateZip.PostalCode.Should().Be("12345");

        var contact = memberLoop.ContactInfo.First();
        contact.ContactFunction.Should().Be("IP");
        contact.ContactName.Should().Be("JANE DOE");
        contact.CommunicationQualifier1.Should().Be("TE");
        contact.CommunicationNumber1.Should().Be("5555555555");
    }

    [Fact]
    public async Task Should_Parse_SE_Segment_Correctly()
    {
        // Arrange & Act
        var transactionSet = await EdiCommon.ParseEdi<Loop834>(_sampleEdi);

        // Assert
        transactionSet.SE.SegmentCount.Should().Be(11);
        transactionSet.SE.TransactionSetControlNumber.Should().Be("0001");
    }

    [Fact]
    public async Task Should_Serialize_And_Deserialize_834_Transaction()
    {
        // Arrange
        var originalTransaction = await EdiCommon.ParseEdi<Loop834>(_sampleEdi);
        var originalPretty = EdiCommon.PrettyPrint(originalTransaction);


        // Act
        var serialized = EdiCommon.WriteEdiToString(originalTransaction);
        var deserialized = await EdiCommon.ParseEdi<Loop834>(serialized);
        var deserializedPretty = EdiCommon.PrettyPrint(deserialized);

        // Assert
        originalPretty.Should().Be(deserializedPretty);
    }

    [Fact]
    public void Should_Create_Valid_834_Transaction_Programmatically()
    {
        // Arrange
        var transaction = new Loop834
        {
            ST = new ST_834
            {
                TransactionSetControlNumber = "0001"
            },
            InsuredLoop = new Loop2000
            {
                INS = new INS_2000
                {
                    InsuredIndicator = "Y",
                    IndividualRelationshipCode = "18"
                },
                REFs = new SegmentList<REF_2000>
                {
                    new()
                    {
                        ReferenceQualifier = "0F",
                        ReferenceId = "12345"
                    }
                }
            },
            MemberLoops = new LoopList<Loop2100>
            {
                new()
                {
                    NM1 = new NM1_2100
                    {
                        EntityIdentifierCode = "IL",
                        EntityTypeQualifier = "1",
                        LastName = "DOE",
                        FirstName = "JOHN"
                    }
                }
            },
            SE = new SE_834
            {
                SegmentCount = 11,
                TransactionSetControlNumber = "0001"
            }
        };

        // Act
        var serialized = EdiCommon.WriteEdiToString(transaction);

        // Assert 
        serialized.Should().NotBeNullOrEmpty();
        serialized.Should().Contain("ST*834*0001");
        serialized.Should().Contain("INS*Y*18");
        serialized.Should().Contain("NM1*IL*1*DOE*JOHN");
        serialized.Should().Contain("SE*11*0001");
    }
}