using EdiSource.Domain.SourceGeneration;
using EdiSource.Loops;

namespace EdiSource.Segments;

[SegmentGenerator<Loop2000>("DTP", null)]
public partial class Loop2000_DTP;