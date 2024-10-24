using System.Collections.Immutable;
using EdiSource.Generator.Helper;
using Microsoft.CodeAnalysis;

namespace EdiSource.Generator.LoopGen;

public partial class LoopGenerator
{
    public static class ChannelConstructorGenerator
    {
        public static string Generate(string className, string namespaceName,
            HashSet<string> usings,
            ImmutableArray<(string Name, string Attribute, IPropertySymbol Property)> orderedEdiItems,
            string parent)
        {
            var cw = new CodeWriter();

            cw.AppendLine("#nullable enable");

            cw.AddUsing("System.Diagnostics.CodeAnalysis");
            cw.AddUsing("System.Threading.Channels");
            foreach (var @using in usings) cw.AddUsing(@using);

            cw.AppendLine();
            using (cw.StartNamespace(namespaceName))
            {
                using (cw.StartClass(className, []))
                {
                    cw.AppendLine("""
                                  [SuppressMessage("CodeQuality", "IDE0060:Remove unused parameter", Justification = "Empty constructor is required")]
                                  """);
                    using (cw.StartConstructor(className))
                    {
                    }

                    cw.AppendLine();

                    using (cw.StartConstructor(className,
                               arguments:
                               [
                                   "ChannelReader<Segment> segmentReader", $"{parent}? parent = null"
                               ]))
                    {
                        cw.AppendLine("var loop = InitializeAsync(segmentReader, parent).GetAwaiter().GetResult();");
                        foreach (var item in orderedEdiItems) cw.AppendLine($"{item.Name} = loop.{item.Name};");
                    }

                    cw.AppendLine();

                    using (cw.AppendBlock(
                               $"public static Task<{className}> InitializeAsync(ChannelReader<Segment> segmentReader, ILoop? parent)"))
                    {
                        using (var _ = cw.AddIf("parent is null"))
                        {
                            cw.AppendLine($"return InitializeAsync(segmentReader, ({parent}?) null);");
                        }

                        cw.AppendLine();

                        using (cw.AddIf($"parent is not {parent} typedParent"))
                        {
                            cw.AppendLine(
                                $"""throw new ArgumentException($"Parent must be of type {parent}");""");
                        }

                        cw.AppendLine();

                        cw.AppendLine("return InitializeAsync(segmentReader, typedParent);");
                    }

                    cw.AppendLine();
                    using (cw.AppendBlock(
                               $"public static async Task<{className}> InitializeAsync(ChannelReader<Segment> segmentReader, {parent}? parent)"))
                    {
                        cw.AppendLine($"var loop = new {className}();");
                        cw.AppendLine();

                        if (className != parent)
                        {
                            cw.AppendLine("loop.Parent = parent;");
                            cw.AppendLine();
                        }

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

                        cw.AppendLine();
                        cw.AppendLine("return loop;");
                    }
                }
            }

            return cw.ToString();
        }

        private static void GenerateHeaderOrFooter(string className,
            ImmutableArray<(string Name, string Attribute, IPropertySymbol Property)> items, CodeWriter cw)
        {
            foreach (var item in items)
                cw.AppendLine(
                    $"loop.{item.Name} = await SegmentLoopFactory<{item.Property.Type.ToString().Replace("?", "")}, {className}>.CreateAsync(segmentReader, loop);");
        }

        private static void GenerateBody(string className,
            ImmutableArray<(string Name, string Attribute, IPropertySymbol Property)> items, CodeWriter cw)
        {
            using (cw.AddWhile("await segmentReader.WaitToReadAsync()"))
            {
                foreach (var (name, attribute, property) in items)
                {
                    var typeName = ((INamedTypeSymbol)property.Type).TypeArguments is { Length: > 0 } named
                        ? named[0]
                        : property.Type;

                    using var _ = cw.AddIf($"await ISegmentIdentifier<{typeName.ToString().Replace("?", "")}>.MatchesAsync(segmentReader)");

                    cw.AppendLine(attribute switch
                    {
                        SegmentAttribute or Segment =>
                            $"loop.{name} = await SegmentLoopFactory<{typeName.ToString().Replace("?", "")}, {className}>.CreateAsync(segmentReader, loop);",
                        SegmentListAttribute or SegmentList =>
                            $"loop.{name}.Add(await SegmentLoopFactory<{typeName.ToString().Replace("?", "")}, {className}>.CreateAsync(segmentReader, loop));",
                        LoopAttribute or Loop =>
                            $"loop.{name} = await {typeName.ToString().Replace("?", "")}.InitializeAsync(segmentReader, loop);",
                        LoopListAttribute or LoopList =>
                            $"loop.{name}.Add(await {typeName.ToString().Replace("?", "")}.InitializeAsync(segmentReader, loop));",
                        OptionalSegmentFooter or OptionalSegmentFooterAttribute =>
                            $"loop.{name} = SegmentLoopFactory<{typeName.ToString().Replace("?", "")}, {className}>.Create(segments, this);",
                        _ => string.Empty
                    });

                    cw.AppendLine(attribute is OptionalSegmentFooter or OptionalSegmentFooterAttribute
                        ? "break;"
                        : "continue;");
                }

                cw.AppendLine();
                cw.AppendLine("break;");
            }
        }
    }
}