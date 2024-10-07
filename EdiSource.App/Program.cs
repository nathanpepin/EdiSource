using Dumpify;
using EdiSource.Domain;
using EdiSource.Domain.IO.Parser;
using EdiSource.Domain.IO.Serializer;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Validation;
using EdiSource.Domain.Validation.Validator;
using EdiSource.Loops;
using Spectre.Console;

var input =
    """
    ST*123~
    REF*A~
    REF*B~
    DTP*1~
    INS*A~
    NM1*1~
    NM1*2~
    SE*123~
    """;

var ts = await new EdiParser<TransactionSet>().ParseEdi(input);

var vr = EdiCommon.Validate(ts);

vr.DumpConsole();
