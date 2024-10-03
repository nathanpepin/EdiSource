using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using VerifyXunit;

namespace EdiSource.Generator.Tests;

public static class TestHelper
{
    public static Task Verify<T>(string source) where T : IIncrementalGenerator, new()
    {
        var generator = new T();

        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        var compilation = CSharpCompilation.Create(nameof(LoopConstructorGeneratorTests),
            [CSharpSyntaxTree.ParseText(source)],
            [MetadataReference.CreateFromFile(typeof(object).Assembly.Location)]);

        driver = driver.RunGenerators(compilation);

        return Verifier.Verify(driver);
    }
}