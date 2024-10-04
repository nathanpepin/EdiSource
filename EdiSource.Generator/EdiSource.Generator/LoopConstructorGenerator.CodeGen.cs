using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace EdiSource.Generator;

public partial class LoopConstructorGenerator
{
    public static string GenerateSourceCode(string className, string namespaceName,
        ImmutableArray<(string Name, string Attribute, IPropertySymbol Property)> orderedEdiItems)
    {
        var cw = new CodeWriter();

        cw.AddUsing("System");
        cw.AddUsing("System.Collections.Generic");
        cw.AddUsing("EdiSource.Domain.Segments");
        cw.AddUsing("EdiSource.Domain.Loop");
        cw.AddUsing("EdiSource.Domain.Identifiers");
        cw.AppendLine();
        using (var ns = cw.StartNamespace(namespaceName))
        {
            using (var cs = cw.StartClass(className))
            {
                using (var con = cw.StartConstructor(className, arguments: ["Queue<ISegment> segments", $"TransactionSet? parent = null"]))
                {
                    var headerItems = orderedEdiItems
                        .Where(x => LoopAggregation.Header.Contains(x.Attribute))
                        .ToImmutableArray();
                    GenerateHeaderOrFooter(className, headerItems, cw);

                    var bodyItems = orderedEdiItems
                        .Where(x => LoopAggregation.Body.Contains(x.Attribute))
                        .ToImmutableArray();
                    if (bodyItems.Length > 0)
                    {
                        cw.AppendLine();
                        GenerateBody(className, bodyItems, cw);
                    }

                    var footerItems = orderedEdiItems
                        .Where(x => LoopAggregation.Footer.Contains(x.Attribute))
                        .ToImmutableArray();

                    if (footerItems.Length > 0)
                    {
                        cw.AppendLine();
                        GenerateHeaderOrFooter(className, footerItems, cw);
                    }
                }
            }
        }

        return cw.ToString();
    }

    private static void GenerateHeaderOrFooter(string className, ImmutableArray<(string Name, string Attribute, IPropertySymbol Property)> items, CodeWriter cw)
    {
        foreach (var item in items)
        {
            cw.AppendLine(
                $"{item.Name} = SegmentLoopFactory<{item.Property.Type}, {className}>.Create(segments, this);");
        }
    }

    private static void GenerateBody(string className, ImmutableArray<(string Name, string Attribute, IPropertySymbol Property)> items, CodeWriter cw)
    {
        using (_ = cw.AddWhile("segments.Count > 0"))
        {
            foreach (var (name, attribute, property) in items)
            {
                var typeName = ((INamedTypeSymbol)property.Type).TypeArguments is { Length: > 0 } named
                    ? named[0]
                    : property.Type;

                using var i = cw.AddIf($"ISegmentIdentifier<{typeName}>.Matches(segments)");

                cw.AppendLine(attribute switch
                {
                    SegmentAttribute or Segment => $"{name} = SegmentLoopFactory<{typeName}, {className}>.Create(segments, this);",
                    SegmentListAttribute or SegmentList => $"{name}.Add(SegmentLoopFactory<{typeName}, {className}>.Create(segments, this));",
                    LoopAttribute or Loop => $"{name} = new {property.Type}(segments, this);",
                    LoopListAttribute or LoopList => $"{name}.Add(new {typeName}(segments, this));",
                    OptionalSegmentFooter or OptionalSegmentFooterAttribute =>
                        $"""
                         {name} = SegmentLoopFactory<{typeName}, {className}>.Create(segments, this);
                         break;
                         """,
                    _ => string.Empty
                });

                cw.AppendLine("continue;");
            }

            cw.AppendLine("break;");
        }
    }
}