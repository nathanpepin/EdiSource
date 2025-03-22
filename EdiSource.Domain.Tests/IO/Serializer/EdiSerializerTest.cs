namespace EdiSource.Domain.Tests.IO.Serializer;

[TestSubject(typeof(EdiSerializer))]
public class EdiSerializerTest
{
    private const string Source =
        """
        ISA*00*          *00*          *ZZ*SENDER         *ZZ*RECEIVER       *200901*1319*^*00501*000000905*0*P*:~
        GS*09*343*343*333*343*3*3*3*34~
        ST*834*ABCD~
        REF*A~
        REF*B~
        DTP*1*D8*2024e0106~
        INS*A~
        NM1*1~
        NM1*2~
        SE*123~
        GE*0*098~
        IEA*1*123~
        """;

    [Fact]
    public Task WriteToPrettyString_Should_PrintLoopCorrectly()
    {
        //Arrange
        var serializer = new EdiSerializer();

        //Act
        var envelope = EdiCommon.ParseEdi<InterchangeEnvelope>(Source)
            .ContinueWith(x => serializer.WriteToPrettyString(x.Result));

        return Verify(envelope);
    }
}