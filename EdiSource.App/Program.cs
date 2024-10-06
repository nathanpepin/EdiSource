using EdiSource.Domain.IO.Parser;
using EdiSource.Domain.IO.Serializer;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Validation;
using EdiSource.Domain.Validation.Validator;
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

var v = ts.Validate();

foreach (var i in v.ValidationMessages)
    Console.WriteLine(i);
//
// Console.WriteLine(
// new EdiSerializer().WriteToPrettyString(ts));