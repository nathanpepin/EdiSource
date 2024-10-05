using EdiSource.Domain.SourceGeneration;
using EdiSource.Loops;

namespace EdiSource.Segments;

[SegmentGenerator<TransactionSet>("SE", null)]
public partial class TS_SE;