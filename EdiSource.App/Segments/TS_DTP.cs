using EdiSource.Domain.SourceGeneration;
using EdiSource.Loops;

namespace EdiSource.Segments;

[SegmentGenerator<TransactionSet>("DTP", null)]
public partial class TS_DTP;