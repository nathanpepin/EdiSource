using System.Threading.Tasks;
using EdiSource.Generator.Tests.Utils;
using EdiSource.Generator.ValidationGen;
using VerifyXunit;
using Xunit;

namespace EdiSource.Generator.Tests.ValidationTests;

public sealed class ValidationGeneratorTests
{
    private const string Source =
        """
        using EdiSource.Domain.SourceGeneration;
        using EdiSource.Domain.Validation.Data;
        using EdiSource.Domain.Validation.Factory;
        using EdiSource.Domain.Validation.SourceGeneration;
        using EdiSource.Loops;
        
        namespace EdiSource.Segments;
        
        [ElementLength(0, 1, 3, 3)]
        public partial class TS_SE
        {
        }
        """;

    [Fact]
    public Task TestA()
    {
        var driver = TestHelperFunctions.Verify<ValidationGenerator>(Source);
        return Verifier.Verify(driver);
    }
}