//HintName: Test_INS.Implementation.g.cs
#nullable enable
using System.Collections.Generic;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Segments;

namespace EdiSource.Loops
{
    public partial class Test_INS : Segment, ISegment<Test_INS>, ISegmentIdentifier<Test_INS>
    {
        public Test? Parent { get; set; }
        public static (string Primary, string? Secondary) EdiId => ("INS", null)
    }
}