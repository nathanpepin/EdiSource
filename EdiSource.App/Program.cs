using EdiSource.Domain;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop.Extensions;
using EdiSource.Domain.Standard.Loops;
using EdiSource.Domain.Validation.Data;
using EdiSource.Loops;
using EdiSource.Segments;


var input =
    """
    ISA*i0*          *00*          *ZZ*SENDER         *ZZ*RECEIVER       *200901*1319*^*00501*000000905*0*P*:~
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

InterchangeEnvelope.TransactionSetDefinitions.Add(_834.Definition);

var env = await EdiCommon.ParseEdi<InterchangeEnvelope>(input);
// var text = EdiCommon.WriteEdiToString(env);
// var prety = EdiCommon.PrettyPrint(env);
// var validation = EdiCommon.Validate(env);

var v = EdiCommon.Validate(env);

var vf = IUserValidation<TS_DTP>.UserValidations;

await ValidationMessageTable.ValidationsTableToFile(new FileInfo("Validations.txt"), v);
