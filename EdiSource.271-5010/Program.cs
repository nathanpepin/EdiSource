const string ediContent =
    """
    ISA*00*          *00*          *ZZ*TESTPAYER      *30*111111111111111*210319*1257*^*00501*100000001*0*P*:~
    GS*HB*000000000000124*1036809856*20210319*12572729*100000001*X*005010X279A1~
    ST*271*00001*005010X279A1~
    BHT*0022*11*162319870*20210319*12572729~
    HL*1**20*1~
    NM1*PR*2*DefaultPayerName*****PI*TESTPAYER~
    PER*IC**UR*WWW.HEALTHCARE1.COM~
    HL*2*1*21*1~
    NM1*1P*2*SAMPLE PROVIDER*****XX*1558444216~
    HL*3*2*22*0~
    TRN*1*fdd38afb-9e48-4fd6-9de2-0bc1e8539824*1562291693*PNT~
    NM1*IL*1*LASTNAME*MARY*X***MI*87878787~
    N3*3 ROAD ST~
    N4*VILLE*DE*111150001~
    DMG*D8*19000101*F~
    EB*U**30~
    LS*2120~
    NM1*VN*2*SUPPLEMENTAL PLAN~
    LE*2120~
    EB*X~
    LS*2120~
    NM1*1P*2*PROVIDER*****XX*1558444216~
    LE*2120~
    SE*22*00001~
    GE*1*100000001~
    IEA*1*100000001~
    """;

// 1. Register 271 transaction set definition
Console.WriteLine("Registering 271 transaction set definition...");
InterchangeEnvelope.TransactionSetDefinitions.Add(_271.Definition);

// 2. Parse the EDI content into an interchange envelope
Console.WriteLine("Parsing 271 EDI content...");
var (envelope, separators) = await EdiCommon.ParseEdiEnvelope(ediContent);

// 3. Write the interchange envelope to a string
Console.WriteLine("Writing 271 EDI content to string...");
var ediString = EdiCommon.PrettyPrint(envelope);

Console.WriteLine("--------------------");
Console.WriteLine(ediString);