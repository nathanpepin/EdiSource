using System.Threading.Tasks;
using EdiSource.Generator.Tests.Utils;
using VerifyXunit;
using Xunit;

namespace EdiSource.Generator.Tests.LoopGenerator;

public class LoopGeneratorTests
{
    private const string Source =
        """
        namespace Test;

        [LoopGenerator<TransactionSet, TransactionSet, TS_ST>]
        public partial class TransactionSet
        {
            [SegmentHeader] public TS_ST ST { get; set; } = default!;
        
            [SegmentList] public SegmentList<TS_REF> REFs { get; set; } = [];
        
            [Segment] public TS_DTP DTP { get; set; }
        
            [Loop] public Loop2000 Loop2000 { get; set; }
        
            [LoopList] public LoopList<Loop2100> Loop2100s { get; set; } = [];
        
            [OptionalSegmentFooter] public TS_FSE SE { get; set; }
        
            [SegmentFooter] public TS_SE SE { get; set; }
        }
        """;

    [Fact]
    public Task TestA()
    {
        var driver = TestHelperFunctions.Verify<LoopGen.LoopGenerator>(Source);
        return Verifier.Verify(driver);
    }
}