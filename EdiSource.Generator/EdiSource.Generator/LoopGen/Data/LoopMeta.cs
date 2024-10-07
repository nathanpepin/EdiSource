using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EdiSource.Generator.LoopGen.Data;

public record struct LoopMeta(
    ClassDeclarationSyntax ClassDeclarationSyntax,
    string Parent,
    string Self,
    string Id);