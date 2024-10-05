using System.Runtime.CompilerServices;
using DiffEngine;
using Microsoft;
using VerifyTests;

namespace EdiSource.Generator.Tests;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        VerifySourceGenerators.Initialize();
    }
}