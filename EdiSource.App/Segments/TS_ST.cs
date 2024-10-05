using EdiSource.Domain.SourceGeneration;
using EdiSource.Loops;

namespace EdiSource.Segments;

[SegmentGenerator<TransactionSet>("ST", null)]
public partial class TS_ST;