using EdiSource.Domain.Identifiers;
using EdiSource.Domain.IO.EdiReader;
using EdiSource.Domain.IO.EdiWriter;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Seperator;

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

var segments = new EdiReader()
    .ReadEdiSegments(input, Separators.DefaultSeparators)
    .ToArray();
//
// var ts = new TransactionSet(new Queue<ISegment>(segments));
// // ;
//
//  var a = ts.ST.Parent;
//
// Console.WriteLine(ts.WriteToStringBuilder().ToString());

return;