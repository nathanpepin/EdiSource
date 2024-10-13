using Dumpify;
using EdiSource.Domain;
using EdiSource.Domain.Identifiers;
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
    ISA*123~
    GS*09~
    ST*123~
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
var j = InterchangeEnvelope.TransactionSetDefinitions;
;

var ts = await new EdiParser<InterchangeEnvelope>().ParseEdiEnvelope(input);
;
//
// IUserValidation<Loop2000>.UserValidations.Add(x => [ValidationFactory.CreateInfo(new Segment(), "Help")]);
// IUserValidation<Loop2000_DTP>.UserValidations.Add(x => []);
//
// var j = IUserValidation<Loop2000>.UserValidations;
// var jj = IUserValidation<TransactionSet>.UserValidations;
//
//
// var dtp = ts.FindEdiElement<TS_DTP>()[0].Date;

;