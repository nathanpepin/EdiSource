using System.Collections.Immutable;
using System.Text;
using EdiSource.Generator.Helper;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using CSharpExtensions = Microsoft.CodeAnalysis.CSharp.CSharpExtensions;

namespace EdiSource.Generator.ValidationGen;

[Generator(LanguageNames.CSharp)]
public class ValidationGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<ClassDeclarationSyntax> classDeclarations =
            context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (s, _) => IsSyntaxTargetForGeneration(s),
                    transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
                .Where(static m => m is not null)!;

        IncrementalValueProvider<(Compilation, ImmutableArray<ClassDeclarationSyntax>)> compilationAndClasses
            = context.CompilationProvider.Combine(classDeclarations.Collect());

        context.RegisterSourceOutput(compilationAndClasses,
            static (spc, source) => Execute(source.Item1, source.Item2, spc));
    }

    private static bool IsSyntaxTargetForGeneration(SyntaxNode node)
        => node is ClassDeclarationSyntax { AttributeLists: { Count: > 0 } };

    private static ClassDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
    {
        var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

        foreach (var attributeList in classDeclarationSyntax.AttributeLists)
        {
            foreach (var attribute in attributeList.Attributes)
            {
                var attributeName = attribute.Name.ToString();
                if (IsTargetAttribute(attributeName))
                {
                    return classDeclarationSyntax;
                }
            }
        }

        return null;
    }

    private static bool IsTargetAttribute(string attributeName)
    {
        return attributeName switch
        {
            "ElementLength" or "ElementLengthAttribute" or
                "Empty" or "EmptyAttribute" or
                "HasLength" or "HasLengthAttribute" or
                "IsOneOfValues" or "IsOneOfValuesAttribute" or
                "NotEmpty" or "NotEmptyAttribute" or
                "NotOneOfValues" or "NotOneOfValuesAttribute" or
                "RequiredDataElements" or "RequiredDataElementsAttribute" or
                "SegmentElementLength" or "SegmentElementLengthAttribute" => true,
            _ => false
        };
    }

    private static void Execute(Compilation compilation, ImmutableArray<ClassDeclarationSyntax> classes,
        SourceProductionContext context)
    {
        if (classes.IsDefaultOrEmpty)
        {
            return;
        }

        foreach (var classDeclaration in classes)
        {
            var semanticModel = compilation.GetSemanticModel(classDeclaration.SyntaxTree);
            var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration);

            if (classSymbol == null) continue;

            CodeWriter cw = new();
            cw.AppendLine("/*");

            using (cw.StartNamespace(classSymbol.ContainingNamespace.ToDisplayString()))
            {
                foreach (var @using in Usings)
                    cw.AddUsing(@using);

                using (cw.StartClass(classDeclaration.Identifier.Text, ["ISourceGeneratorValidatable"]))
                {
                    ProcessAttributes(classDeclaration, cw, compilation);
                }

                cw.AppendLine("*/");
            }


            context.AddSource($"{classSymbol.Name}.Validation.g.cs", SourceText.From(cw.ToString(), Encoding.UTF8));
        }
    }

    private static void ProcessAttributes(ClassDeclarationSyntax classDeclarationSyntax, CodeWriter cw, Compilation compilation)
    {
        foreach (var attributeList in classDeclarationSyntax.AttributeLists)
        {
            foreach (var attribute in attributeList.Attributes)
            {
                var attributeName = attribute.Name.ToString();
                var arguments = attribute.ArgumentList?.Arguments.Select(a => 
                                    a.ToString()).ToArray() ?? [];

               var symbol = compilation.GetSemanticModel(attribute.SyntaxTree);
               
                switch (attributeName)
                {
                    case "ElementLength":
                    case "ElementLengthAttribute":
                        if (arguments.Length >= 4)
                        {
                            var a = attribute.ArgumentList?.Arguments.ToString();
                            cw.AppendLine($"new ElementLengthAttribute({a})");
                            
                            var dataElement = int.Parse(arguments[0]);
                            var compositeElement = int.Parse(arguments[1]);
                            var min = int.Parse(arguments[2]);
                            var max = int.Parse(arguments[3]);

                            if (min == max)
                            {
                                using (cw.AddIf($$"""
                                                  GetCompositeElementOrNull({{dataElement}}, {{compositeElement}}) is {} item && is { Length: {{min}} }
                                                  """)) ;
                            }
                            else
                            {
                                using (cw.AddIf($$"""
                                                  GetCompositeElementOrNull({{dataElement}}, {{compositeElement}}) is {} item && is { Length: >= {{min}} and <= {{max}} }
                                                  """))
                                {
                                }
                            }
                        }

                        break;
                }
            }
        }
    }
}

public readonly record struct ValidationData
{
    public ValidationType Type { get; }
}

public enum ValidationType
{
    Unknown = 0,
    ElementLength,
    Empty,
    HasLength,
    IsOneOfValues,
    NotEmpty,
    NotOneOfValues,
    RequiredDataElements,
    SegmentElementLength
}