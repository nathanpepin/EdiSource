using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EdiSource.Generator.EdiElementGenerator;

public record struct GeneratorItem(ClassDeclarationSyntax ClassDeclarationSyntax, string Parent, string Self, string Id);