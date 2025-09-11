using EdiSource._270_5010.TransactionSet;
using EdiSource.Domain;
using EdiSource.Domain.Standard.Loops.ISA;
using Xunit.Abstractions;

namespace EdiSource._270_5010.Tests;

public sealed class MainTests(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public async Task Read270()
    {
        const string ediContent =
            """
            ISA*00*          *00*          *ZZ*TESTPAYER      *30*111111111111111*210319*1257*^*00501*100000001*0*P*:~
            GS*HB*000000000000124*1036809856*20210319*12572729*100000001*X*005010X279A1~
            ST*270*0001*005010X279A1~
            BHT*0022*01*X*20250912*1521*RT~
            HL*1**20*1~
            NM1*P5*2*XX*XXXX*X**XXXXX*NI*XXXXXX~
            HL*2*1*21*1~
            NM1*FA*1*XXX*X*XXXXX**XXXX*XV*XXXXXX~
            REF*N7*XX*XXXXXX~
            N3*X*XXXX~
            N4*XXXXXXX*XX*XXXX*XXX~
            PRV*BI*PXC*XXXXXX~
            HL*3*2*22*1~
            TRN*1*XXXXX*XXXXXXXXXX*XX~
            NM1*IL*1*X*XX*XXXXXX**XX*II*XX~
            REF*Y4*XXXXX~
            N3*XXX*XXX~
            N4*XXXXXX*XX*XXXXXXX*XX~
            PRV*R*EI*XXX~
            DMG*D8*XXX*M~
            INS*Y*18***************0000000~
            HI*BK>XXXXX*BF>XX*BF>X*BF>XXXXX*ABF>XXXXX*BF>XXXX*ABF>XXXX*ABF>X~
            DTP*102*RD8*XXXXXX~
            EQ*53*ID>XXXXX>XX>XX>XX>XX*FAM**00>0>00>0~
            AMT*R*0000000~
            AMT*PB*000000~
            III*ZZ*XXXXX~
            REF*9F*XXXXXX~
            DTP*291*RD8*XXXXX~
            HL*4*3*23*0~
            TRN*1*XXX*XXXXXXXXXX*X~
            NM1*03*1*XX*XXXXXX*X**X~
            REF*IF*XXXXX~
            N3*X*XXX~
            N4*XXXXXXX*XX*XXXXXXXX*XXX~
            PRV*SK*9K*XXXX~
            DMG*D8*XX*F~
            INS*N*01***************0000~
            HI*ABK>X*ABF>XXXXXX*ABF>XXXXXX*ABF>XXX*BF>XXXX*BF>X*ABF>XXXXX*BF>X~
            DTP*102*RD8*XXXXXX~
            EQ*B1*ID>XXXXX>XX>XX>XX>XX***0>0>00>00~
            III*ZZ*XXX~
            REF*9F*XX~
            DTP*291*D8*XX~
            SE*43*0001~
            GE*1*100000001~
            IEA*1*100000001~
            """;

        // 1. Register 270 transaction set definition
        testOutputHelper.WriteLine("Registering 270 transaction set definition...");
        InterchangeEnvelope.TransactionSetDefinitions.Add(_270_5010_EligibilityBenefitInquiry.Definition);

        // 2. Parse the EDI content into an interchange envelope
        testOutputHelper.WriteLine("Parsing 270 EDI content...");
        var (envelope, _) = await EdiCommon.ParseEdiEnvelope(ediContent);

        // 3. Write the interchange envelope to a string
        testOutputHelper.WriteLine("Writing 270 EDI content to string...");
        var ediString = EdiCommon.PrettyPrint(envelope);

        testOutputHelper.WriteLine("--------------------");
        testOutputHelper.WriteLine(ediString);
    }
}