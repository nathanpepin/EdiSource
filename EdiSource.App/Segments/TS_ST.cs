using EdiSource.Domain.SourceGeneration;
using EdiSource.Domain.Standard.Segments.STData;
using EdiSource.Loops;

namespace EdiSource.Segments;

[SegmentGenerator<_834, ST>("ST", "834")]
public partial class TS_ST : ST;