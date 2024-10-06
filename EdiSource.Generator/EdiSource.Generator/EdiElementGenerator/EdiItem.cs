using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EdiSource.Generator.EdiElementGenerator;

public record struct EdiItem(
    ClassDeclarationSyntax ClassDeclarationSyntax,
    string Parent,
    string Self,
    string Id);