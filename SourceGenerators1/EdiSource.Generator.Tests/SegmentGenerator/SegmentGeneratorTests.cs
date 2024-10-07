using System.Threading.Tasks;
using EdiSource.Generator.Tests.Utils;
using VerifyXunit;
using Xunit;

namespace EdiSource.Generator.Tests.SegmentGenerator;

public class SegmentGeneratorTests
{
    private const string Source =
        """
        using EdiSource.Domain.Identifiers;
        using EdiSource.Domain.Loop;
        using EdiSource.Domain.Segments;
        using EdiSource.Domain.SourceGeneration;
        using EdiSource.Segments;

        namespace EdiSource.Loops;

        [SegmentGenerator<Test>("INS", null)]
        public class Test_INS : Segment, ISegment<Test>, ISegmentIdentifier<Test_INS>
        {
            public Test? Parent { get; }
            public static (string Primary, string? Secondary) EdiId => ("INS", null);
        }
        """;

    [Fact]
    public Task TestA()
    {
        var driver = TestHelperFunctions.Verify<SegmentGen.SegmentGenerator>(Source);
        return Verifier.Verify(driver);
    }
}