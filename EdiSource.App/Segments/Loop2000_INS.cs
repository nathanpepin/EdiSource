using EdiSource.Domain.SourceGeneration;
using EdiSource.Loops;

namespace EdiSource.Segments;

[SegmentGenerator<Loop2000>("INS", null)]
public partial class Loop2000_INS;