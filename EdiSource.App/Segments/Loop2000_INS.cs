using EdiSource.Domain.SourceGeneration;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.SourceGeneration;
using EdiSource.Loops;

namespace EdiSource.Segments;

[BeDate(ValidationSeverity.Critical, 2, 0)]
[SegmentGenerator<Loop2000>("INS", null)]
public partial class Loop2000_INS;