using System.Collections.Immutable;
using EdiSource.Generator.LoopGen.Data;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EdiSource.Generator.Helper;

public static class HelperFunctions
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

    public static LoopMeta PredicateOnClassAttributes(GeneratorSyntaxContext context, ImmutableArray<string> items)
    {
        var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

        var attribute = GetAttributeSyntax(classDeclarationSyntax, [..items])!;

        var typeArgumentListSyntaxes = attribute
            .DescendantNodes().OfType<TypeArgumentListSyntax>().ToList();

        var parent = typeArgumentListSyntaxes[0].Arguments[0].ToString();
        var self = typeArgumentListSyntaxes[0].Arguments[1].ToString();
        var id = typeArgumentListSyntaxes[0].Arguments[2].ToString();

        var isTransactionSet =
            attribute
                .DescendantNodes()
                .OfType<AttributeArgumentSyntax>()
                .FirstOrDefault()
                ?.Expression is { } expression
            && context.SemanticModel.GetConstantValue(expression) is var value
            && (bool)value.Value!;

        return new LoopMeta(
            classDeclarationSyntax,
            parent.RemoveQuestionMark(),
            self.RemoveQuestionMark(),
            id.RemoveQuestionMark(),
            isTransactionSet);
    }

    private static string RemoveQuestionMark(this string it)
    {
        return it.EndsWith("?") ? it[..^1] : it;
    }

    private static string? RemoveQuestionMarkNull(this string? it)
    {
        return it is null ? null : RemoveQuestionMark(it);
    }

    public static (ClassDeclarationSyntax, string loop, ImmutableArray<string> args, string? subType)
        PredicateOnClassAttributesClassParent(GeneratorSyntaxContext context, ImmutableArray<string> items)
    {
        var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

        var attribute = GetAttributeSyntax(classDeclarationSyntax, [..items])!;

        var typeArgumentListSyntaxes = attribute
            .DescendantNodes().OfType<TypeArgumentListSyntax>().ToArray();

        var parent = typeArgumentListSyntaxes[0].Arguments[0].ToString();

        var subType =
            typeArgumentListSyntaxes[0].Arguments.Count == 2
                ? typeArgumentListSyntaxes[0].Arguments[1].ToString()
                : null;

        var args = attribute.DescendantNodes().OfType<AttributeArgumentSyntax>()
            .Select(x => x.Expression.ToString())
            .ToImmutableArray();

        return (classDeclarationSyntax, parent, args, subType);
    }

    public static TypeSyntax? GetSegmentGeneratorSubType(GeneratorSyntaxContext context)
    {
        var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

        var attribute = GetAttributeSyntax(classDeclarationSyntax, [.. LoopAggregation.SegmentGeneratorNames]);

        if (attribute is null) return null;
        
        var typeArgumentListSyntaxes = attribute
            .DescendantNodes().OfType<TypeArgumentListSyntax>().ToArray();

        if (typeArgumentListSyntaxes.Length == 0)
            return null;
        
        return
            typeArgumentListSyntaxes[0].Arguments.Count == 2
                ? typeArgumentListSyntaxes[0].Arguments[1]
                : null;
    }

    public static bool IsSyntaxTargetForGeneration(SyntaxNode node, ImmutableArray<string> items)
    {
        return node is ClassDeclarationSyntax classDeclarationSyntax
               && HasAttribute(classDeclarationSyntax, [.. items]);
    }

    public static AttributeSyntax? GetAttributeSyntax(ClassDeclarationSyntax classDeclaration, string[] attributeNames)
    {
        foreach (var attributeList in classDeclaration.AttributeLists)
        foreach (var attribute in attributeList.Attributes)
        {
            var attributeName = attribute.Name.ToString();
            foreach (var name in attributeNames)
                if (attributeName.StartsWith($"{name}<"))
                    return attribute;
        }

        return null;
    }

    public static bool HasAttribute(ClassDeclarationSyntax classDeclaration, string[] attributeNames)
    {
        foreach (var attributeList in classDeclaration.AttributeLists)
        foreach (var attribute in attributeList.Attributes)
        {
            var attributeName = attribute.Name.ToString();
            foreach (var name in attributeNames)
                if (attributeName.StartsWith($"{name}<"))
                    return true;
        }

        return false;
    }

    public static IEnumerable<UsingDirectiveSyntax> GetUsingStatements(ClassDeclarationSyntax classDeclaration)
    {
        var parent = classDeclaration.Parent;

        while (parent != null && parent is not NamespaceDeclarationSyntax && !(parent is CompilationUnitSyntax))
            parent = parent.Parent;

        return parent switch
        {
            NamespaceDeclarationSyntax namespaceDeclaration => namespaceDeclaration.Usings,
            CompilationUnitSyntax compilationUnit => compilationUnit.Usings,
            _ => []
        };
    }
}