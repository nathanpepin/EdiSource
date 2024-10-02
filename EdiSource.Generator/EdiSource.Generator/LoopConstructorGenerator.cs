using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace EdiSource.Generator
{
    [Generator]
    public class EdiLoopConstructorGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            IncrementalValuesProvider<ClassDeclarationSyntax> classDeclarations = context.SyntaxProvider
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
            => node is ClassDeclarationSyntax { AttributeLists.Count: > 0 };

        private static ClassDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
        {
            var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

            return classDeclarationSyntax;
            
            foreach (var attributeList in classDeclarationSyntax.AttributeLists)
            {
                foreach (var attribute in attributeList.Attributes)
                {
                    if (context.SemanticModel.GetSymbolInfo(attribute).Symbol is IMethodSymbol attributeSymbol)
                    {
                        INamedTypeSymbol attributeContainingTypeSymbol = attributeSymbol.ContainingType;
                        string fullName = attributeContainingTypeSymbol.ToDisplayString();

                        if (fullName == "LoopGeneratorAttribute")
                        {
                            return classDeclarationSyntax;
                        }
                    }
                }
            }

            return null;
        }

        private static void Execute(Compilation compilation, ImmutableArray<ClassDeclarationSyntax> classes,
            SourceProductionContext context)
        {
            if (classes.IsDefaultOrEmpty)
            {
                return;
            }

            var distinctClasses = classes.Distinct();

            foreach (var classDeclaration in distinctClasses)
            {
                var semanticModel = compilation.GetSemanticModel(classDeclaration.SyntaxTree);
                var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol;

                if (classSymbol == null) continue;

                var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();
                var className = classSymbol.Name;
                var properties = classSymbol.GetMembers().OfType<IPropertySymbol>();

                var ediItems = new List<(string Name, string Attribute, IPropertySymbol Property)>();

                foreach (var property in properties)
                {
                    var attribute = GetEdiAttribute(property);
                    if (!string.IsNullOrEmpty(attribute))
                    {
                        ediItems.Add((property.Name, attribute, property));
                    }
                }

                var orderedEdiItems = OrderEdiItems(ediItems);
                var sourceCode = GenerateSourceCode(className, namespaceName, orderedEdiItems);

                context.AddSource($"{className}.Constructor.g.cs", SourceText.From(sourceCode, Encoding.UTF8));
            }
        }

        private static string GetEdiAttribute(IPropertySymbol property)
        {
            var attributes = property.GetAttributes();
            var ediAttribute = attributes.FirstOrDefault(a => IsEdiAttribute(a.AttributeClass?.Name));
            return ediAttribute?.AttributeClass?.Name ?? string.Empty;
        }

        private static bool IsEdiAttribute(string? attributeName)
        {
            return attributeName switch
            {
                "SegmentHeaderAttribute" or "SegmentAttribute" or "SegmentListAttribute" or
                    "LoopAttribute" or "LoopListAttribute" or "SegmentFooterAttribute" => true,
                _ => false
            };
        }

        private static List<(string Name, string Attribute, IPropertySymbol Property)> OrderEdiItems(
            List<(string Name, string Attribute, IPropertySymbol Property)> ediItems)
        {
            return ediItems.OrderBy(item => item.Attribute switch
            {
                "SegmentHeaderAttribute" => 0,
                "SegmentAttribute" => 1,
                "SegmentListAttribute" => 2,
                "LoopAttribute" => 3,
                "LoopListAttribute" => 4,
                "SegmentFooterAttribute" => 5,
                _ => 6
            }).ToList();
        }

        private static string GenerateSourceCode(string className, string namespaceName,
            List<(string Name, string Attribute, IPropertySymbol Property)> orderedEdiItems)
        {
            var sb = new StringBuilder();

            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using EdiSource.Domain.Segments;");
            sb.AppendLine("using EdiSource.Domain.Loop;");
            sb.AppendLine("using EdiSource.Domain.Identifiers;");
            sb.AppendLine();
            sb.AppendLine($"namespace {namespaceName}");
            sb.AppendLine("{");
            sb.AppendLine($"    public partial class {className}");
            sb.AppendLine("    {");
            sb.AppendLine($"        public {className}(Queue<ISegment> segments)");
            sb.AppendLine("        {");

            foreach (var (name, attribute, property) in orderedEdiItems)
            {
                switch (attribute)
                {
                    case "SegmentHeaderAttribute":
                    case "SegmentAttribute":
                    case "SegmentFooterAttribute":
                        sb.AppendLine(
                            $"            {name} = SegmentLoopFactory<{property.Type}, {className}>.Create(segments, this);");
                        break;
                    case "SegmentListAttribute":
                        sb.AppendLine($"            {name} = [];");
                        sb.AppendLine(
                            $"            while (ISegmentIdentifier<{((INamedTypeSymbol)property.Type).TypeArguments[0]}>.Matches(segments))");
                        sb.AppendLine("            {");
                        sb.AppendLine(
                            $"                {name}.Add(SegmentLoopFactory<{((INamedTypeSymbol)property.Type).TypeArguments[0]}, {className}>.Create(segments, this));");
                        sb.AppendLine("            }");
                        break;
                    case "LoopAttribute":
                        sb.AppendLine($"            if (ISegmentIdentifier<{property.Type}>.Matches(segments))");
                        sb.AppendLine("            {");
                        sb.AppendLine($"                {name} = new {property.Type}(segments, this);");
                        sb.AppendLine("            }");
                        break;
                    case "LoopListAttribute":
                        sb.AppendLine($"            {name} = [];");
                        sb.AppendLine(
                            $"            while (ISegmentIdentifier<{((INamedTypeSymbol)property.Type).TypeArguments[0]}>.Matches(segments))");
                        sb.AppendLine("            {");
                        sb.AppendLine(
                            $"                {name}.Add(new {((INamedTypeSymbol)property.Type).TypeArguments[0]}(segments, this));");
                        sb.AppendLine("            }");
                        break;
                }
            }

            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}