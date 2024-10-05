using EdiSource.Domain.SourceGeneration;
using EdiSource.Loops;

namespace EdiSource.Segments;

[SegmentGenerator<TransactionSet>("REF", null)]
public partial class TS_REF;