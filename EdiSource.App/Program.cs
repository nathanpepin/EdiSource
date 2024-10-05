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


var segments = new EdiReader()
    .ReadEdiSegments(input, Separators.DefaultSeparators)
    .ToArray();

using var stream = new MemoryStream();
input.CopyTo(input.ToCharArray().AsSpan());
var strem = new StreamReader(stream);

var channel = Channel.CreateUnbounded<ISegment>(new UnboundedChannelOptions
{
    SingleReader = true,
    SingleWriter = true
});

await Task.WhenAll(
    TransactionSet.InitializeAsync(channel.Reader, null),
    new EdiReader().ReadEdiSegmentsIntoChannelAsync(strem, channel.Writer));

var ts = new TransactionSet(new Queue<ISegment>(segments));

;

return;