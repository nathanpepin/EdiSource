using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace EdiSource.Generator;

public partial class LoopConstructorGenerator
{
    private static string GenerateSourceCode(string className, string namespaceName,
        List<(string Name, string Attribute, IPropertySymbol Property)> orderedEdiItems)
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
                using (var con = cw.StartConstructor(className, arguments: "Queue<ISegment> segments"))
                {
                    GenerateHeaderOrFooter(className, orderedEdiItems, cw, SegmentHeaderAttribute, SegmentHeader);
                    cw.AppendLine();

                    using (var whi = cw.AddWhile("segments.Count > 0"))
                    {
                        GenerateBody(className, orderedEdiItems, cw);
                    }

                    cw.AppendLine();

                    GenerateHeaderOrFooter(className, orderedEdiItems, cw, SegmentFooterAttribute, SegmentFooter);
                }
            }
        }

        return cw.ToString();
    }

    private static void GenerateHeaderOrFooter(string className, List<(string Name, string Attribute, IPropertySymbol Property)> orderedEdiItems, CodeWriter cw,
        string attribute,
        string attribute2)
    {
        var items = orderedEdiItems.Where(x => x.Attribute == attribute || x.Attribute == attribute2)
            .ToArray();

        if (items.Length == 0)
            return;

        foreach (var item in items)
        {
            cw.AppendLine(
                $"{item.Name} = SegmentLoopFactory<{item.Property.Type}, {className}>.Create(segments, this);");
        }
    }

    private static void GenerateBody(string className, List<(string Name, string Attribute, IPropertySymbol Property)> orderedEdiItems, CodeWriter cw)
    {
        var bodyItems = orderedEdiItems
            .Where(x => x.Attribute is SegmentAttribute or SegmentListAttribute or LoopAttribute
                or LoopListAttribute or Segment or SegmentList or Loop
                or LoopList)
            .ToArray();

        if (bodyItems.Length == 0)
            return;

        foreach (var (name, attribute, property) in bodyItems)
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
                _ => string.Empty
            });

            cw.AppendLine("continue;");
        }
    }
}