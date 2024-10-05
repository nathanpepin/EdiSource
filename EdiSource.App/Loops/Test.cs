using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.SourceGeneration;

namespace EdiSource.Loops;

[LoopGenerator<Test, Test_INS>]
public partial class Test
{
    [SegmentHeader] public Test_INS TestIns { get; set; }
}

[SegmentGenerator<Test>("INS", null)]
public partial class Test_INS;