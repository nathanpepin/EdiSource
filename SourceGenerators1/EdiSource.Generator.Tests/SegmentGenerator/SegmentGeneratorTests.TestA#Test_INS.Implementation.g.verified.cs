//HintName: Test_INS.Implementation.g.cs
#nullable enable
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Segments;
using EdiSource.Domain.Separator;
using EdiSource.Loops;
using System.Linq;
using System.Collections.Generic;
using System;

namespace EdiSource.Loops
{
    public partial class Test_INS
    : Segment, ISegment<Test>, ISegmentIdentifier<Test_INS>
    {
        new public Test? Parent { get; set; }
        public static (string Primary, string? Secondary) EdiId => ("INS", null);
    }
}
