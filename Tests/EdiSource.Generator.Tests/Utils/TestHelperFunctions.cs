using EdiSource.Domain.Identifiers;

namespace EdiSource.Generator.Tests.Utils;

public static class TestHelperFunctions
{
    public static readonly Lazy<VerifySettings> Settings = new(() =>
    {
        var settings = new VerifySettings();

        settings.DisableDiff();

        return settings;
    });

    public static GeneratorDriver Verify<T>(string source) where T : IIncrementalGenerator, new()
    {
        var generator = new T();

        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        var compilation = CSharpCompilation.Create(nameof(T),
            [CSharpSyntaxTree.ParseText(source)],
            [
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Attribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(IEdi).Assembly.Location)
            ]);

        return driver.RunGenerators(compilation);
    }
}