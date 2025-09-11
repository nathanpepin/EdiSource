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
            ISA*00* *00* *ZZ*SUBMITTERID    *ZZ*RECEIVERID     *250911*1344*^*00501*000000001*0*P*:~
            GS*HS*SUBMITTERID*RECEIVERID*20250911*1344*1*X*005010X279A1~
            ST*270*0001*005010X279A1~
            BHT*0022*13*PROVIDERREF123*20250911*1344~
            HL*1**20*1~
            NM1*PR*2*UNITED HEALTHCARE*****PI*RECEIVERID~
            HL*2*1*21*1~
            NM1*1P*2*ANYTOWN GENERAL HOSPITAL*****XX*1234567890~
            HL*3*2*22*0~
            TRN*1*PATIENTINQUIRY98765*9876543210~
            NM1*IL*1*SMITH*JOHN*A***MI*111223333~
            DMG*D8*19800515*M~
            DTP*291*D8*20250915~
            EQ*30~
            SE*13*0001~
            GE*1*1~
            IEA*1*000000001~
            """;

        // 1. Register 271 transaction set definition
        testOutputHelper.WriteLine("Registering 271 transaction set definition...");
        InterchangeEnvelope.TransactionSetDefinitions.Add(_270_5010_EligibilityBenefitInquiry.Definition);

        // 2. Parse the EDI content into an interchange envelope
        testOutputHelper.WriteLine("Parsing 271 EDI content...");
        var (envelope, _) = await EdiCommon.ParseEdiEnvelope(ediContent);

        // 3. Write the interchange envelope to a string
        testOutputHelper.WriteLine("Writing 271 EDI content to string...");
        var ediString = EdiCommon.PrettyPrint(envelope);

        testOutputHelper.WriteLine("--------------------");
        testOutputHelper.WriteLine(ediString);
    }
}