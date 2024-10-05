using System.Collections.Immutable;
using System.Linq;
using EdiSource.Generator.Helper;
using Microsoft.CodeAnalysis;

namespace EdiSource.Generator.EdiElementGenerator;

public partial class EdiItemsIncrementalGenerator
{
    public static class QueueConstructorGenerator
    {
        public static string Generate(string className, string namespaceName,
            ImmutableArray<string> usings,
            ImmutableArray<(string Name, string Attribute, IPropertySymbol Property)> orderedEdiItems,
            string parent)
        {
            var cw = new CodeWriter();

            cw.AppendLine("#nullable enable");
            
            cw.AddUsing("System");
            cw.AddUsing("System.Collections.Generic");
            foreach (var @using in usings)
            {
                cw.AddUsing(@using);
            }
            cw.AppendLine();
            using (var ns = cw.StartNamespace(namespaceName))
            {
                using (var cs = cw.StartClass(className))
                {
                    
                    cw.AppendLine($"public {className}(IEnumerable<ISegment> segments, TransactionSet? parent = null)");
                    cw.AppendLine(": this(new Queue<ISegment>(segments), parent)");
                    cw.AppendLine("{}");
                    cw.AppendLine();
                    
                    using (var con = cw.StartConstructor(className, arguments: ["Queue<ISegment> segments", $"{parent}? parent = null"]))
                    {
                        cw.AppendLine("Parent = parent;"); 
                        cw.AppendLine();
                        
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
}