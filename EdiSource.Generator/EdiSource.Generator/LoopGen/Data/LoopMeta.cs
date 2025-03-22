namespace EdiSource.Generator.LoopGen.Data;

public record struct LoopMeta(
    ClassDeclarationSyntax ClassDeclarationSyntax,
    string Parent,
    string Self,
    string Id,
    bool IsTransactionSet);