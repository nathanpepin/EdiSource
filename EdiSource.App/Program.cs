using System.Text;
using System.Threading.Channels;
using Dumpify;
using EdiSource.Domain;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.IO.EdiReader;
using EdiSource.Domain.IO.Parser;
using EdiSource.Domain.IO.Serializer;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Loop.Extensions;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Standard.Loops;
using EdiSource.Domain.Validation;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.Factory;
using EdiSource.Domain.Validation.Validator;
using EdiSource.Loops;
using EdiSource.Segments;
using Spectre.Console;

var input =
    """
    ISA*00*          *00*          *ZZ*SENDER         *ZZ*RECEIVER       *200901*1319*^*00501*000000905*0*P*:~
    GS*09~
    ST*834~
    REF*A~
    REF*B~
    DTP*1*20240106~
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
;