using System.Runtime.CompilerServices;
using VerifyTests;

namespace EdiSource.Generator.Tests.Utils;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        VerifyDiffPlex.Initialize();
        VerifySourceGenerators.Initialize();
    }
}