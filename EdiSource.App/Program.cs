using System.Text;
using System.Threading.Channels;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.IO.EdiReader;
using EdiSource.Domain.IO.EdiWriter;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Seperator;
using EdiSource.Loops;

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

//
// var segments = new EdiReader()
//     .ReadEdiSegments(input, Separators.DefaultSeparators)
//     .ToArray();

// using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(input));
// using var streamReader = new StreamReader(memoryStream);
//
// var channel = Channel.CreateUnbounded<ISegment>();
//
// await Task.WhenAll(
//     new EdiReader().ReadEdiSegmentsIntoChannelAsync(streamReader, channel.Writer),
//     TransactionSet.InitializeAsync(channel.Reader, null));

// var ts = new TransactionSet(new Queue<ISegment>(segments));

var ts = await new EdiParser<TransactionSet>().ParseEdi(input);

var j = ts.FindEdiElement<Loop2000>().ToArray();

Console.WriteLine(ts.PrettyPrintToStringBuilder());

;

return;