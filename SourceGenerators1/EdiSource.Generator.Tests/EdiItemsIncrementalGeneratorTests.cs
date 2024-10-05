using EdiSource.Generator.EdiElementGenerator;
using Microsoft.CodeAnalysis;
using Xunit;

namespace EdiSource.Generator.Tests;

[Generator]
public class EdiItemsIncrementalGeneratorTests
{
    private const string Source =
        """
        using EdiSource.Domain.Identifiers;
        using EdiSource.Domain.Loop;
        using EdiSource.Domain.Segments;
        using EdiSource.Domain.SourceGeneration;
        using EdiSource.Segments;

        namespace EdiSource.Loops;

        [LoopGenerator<TransactionSet, TS_ST>]
        public partial class TransactionSet : ILoop<TransactionSet>, ISegmentIdentifier<TS_ST>,
            ISegmentIdentifier<TransactionSet>
        {
            [SegmentHeader] public TS_ST ST { get; set; } = default!;
        
            [SegmentList] public SegmentList<TS_REF> REFs { get; set; } = [];
        
            [Segment] public TS_DTP DTP { get; set; }
        
            [Loop] public Loop2000 Loop2000 { get; set; }
        
            [LoopList] public LoopList<Loop2100> Loop2100s { get; set; } = [];
        
            [SegmentFooter] public TS_SE SE { get; set; }
        
            ILoop? ILoop.Parent => Parent;
            public TransactionSet? Parent => null;
        
            public static (string Primary, string? Secondary) EdiId => TS_ST.EdiId;
        }
        """;

    [Fact]
    public void TestA()
    {
        TestHelper.Verify<EdiItemsIncrementalGenerator>(Source);
    }
}