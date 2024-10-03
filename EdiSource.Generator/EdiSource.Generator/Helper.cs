using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EdiSource.Generator;

public static class Helper
{
    public static bool IsEdiAttribute(string? attributeName)
    {
        return attributeName switch
        {
            SegmentHeaderAttribute or SegmentHeader => true,
            SegmentAttribute or Segment => true,
            SegmentListAttribute or SegmentList => true,
            LoopAttribute or Loop => true,
            LoopListAttribute or LoopList => true,
            SegmentFooterAttribute or SegmentFooter => true,
            OptionalSegmentFooterAttribute or OptionalSegmentFooter => true,
            _ => false
        };
    }

    public static ImmutableArray<(string Name, string Attribute, IPropertySymbol Property)> OrderEdiItems(
        IEnumerable<(string Name, string Attribute, IPropertySymbol Property)> ediItems)
    {
        return
        [
            ..ediItems.OrderBy(item => item.Attribute switch
            {
                SegmentHeaderAttribute or SegmentHeader => 0,
                SegmentAttribute or Segment => 1,
                SegmentListAttribute or SegmentList => 2,
                LoopAttribute or Loop => 3,
                LoopListAttribute or LoopList => 4,
                OptionalSegmentFooterAttribute or OptionalSegmentFooter => 5,
                SegmentFooterAttribute or SegmentFooter => 6,
                _ => 7
            })
        ];
    }

    public static string GetEdiAttribute(IPropertySymbol property)
    {
        var attributes = property.GetAttributes();
        var ediAttribute = attributes.FirstOrDefault(a => IsEdiAttribute(a.AttributeClass?.Name));
        return ediAttribute?.AttributeClass?.Name ?? string.Empty;
    }

    public static ClassDeclarationSyntax? PredicateOnClassAttributes(GeneratorSyntaxContext context, ImmutableArray<string> items)
    {
        var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

        var attributes = classDeclarationSyntax
            .AttributeLists
            .SelectMany(attributeList => attributeList.Attributes)
            .ToArray();
        foreach (var attribute in attributes)
        {
            var canidates = context
                .SemanticModel
                .GetSymbolInfo(attribute)
                .CandidateSymbols
                .Select(x => x.ContainingType.Name)
                .ToArray();

            var containsName = items.Any(x => canidates.Contains(x));
            if (!containsName) continue;
            
            return classDeclarationSyntax;
        }

        return null;
    }

    public static bool IsSyntaxTargetForGeneration(SyntaxNode node)
        => node is ClassDeclarationSyntax { AttributeLists.Count: > 0 };
}