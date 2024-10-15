using EdiSource.Domain;
using EdiSource.Domain.Loop.Extensions;
using EdiSource.Domain.Standard.Loops;
using EdiSource.Loops;
using EdiSource.Segments;


var input =
    """
    ISA*00*          *00*          *ZZ*SENDER         *ZZ*RECEIVER       *200901*1319*^*00501*000000905*0*P*:~
    GS*09~
    ST*834~
    REF*A~
    REF*B~
    DTP*1*D8*20240106~
    INS*A~
    NM1*1~
    NM1*2~
    SE*123~
    GE*0~
    IEA*1*123~
    """;

InterchangeEnvelope.TransactionSetDefinitions.Add(_834.Definition);

var env = await EdiCommon.ParseEdi<InterchangeEnvelope>(input);
var text = EdiCommon.WriteEdiToString(env);
var prety = EdiCommon.PrettyPrint(env);
var validation = EdiCommon.Validate(env);

var dtp = env.FindEdiElement<Loop2100_NM1>();
;