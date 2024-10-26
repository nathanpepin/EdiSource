using EdiSource.Domain;
using EdiSource.Domain.Identifiers;using EdiSource.Domain.IO.Parser;
using EdiSource.Domain.Standard.Loops.ISA;
using EdiSource.Domain.Validation.Data;
using EdiSource.Loops;

var input =
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
;
//;

var ediId = new EdiId("NO");

var j = (await EdiCommon.ParseIntoSegments(input)).ToArray();
var isa = j[0];
var k = isa.ToString();

var jjj = ediId.MatchesSegment(isa);


InterchangeEnvelope.TransactionSetDefinitions.Add(_834.Definition);

var env = await EdiCommon.ParseEdi<InterchangeEnvelope>(input);

Console.WriteLine(EdiCommon.PrettyPrint(env));

ValidationMessageTablePrinter.PrintColorCodedValidationMessagesTable(EdiCommon.Validate(env));
;