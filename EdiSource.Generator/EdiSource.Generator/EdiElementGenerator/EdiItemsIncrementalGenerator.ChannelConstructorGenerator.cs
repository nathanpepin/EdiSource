using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using EdiSource.Generator.Helper;
using Microsoft.CodeAnalysis;

namespace EdiSource.Generator.EdiElementGenerator;

public partial class EdiItemsIncrementalGenerator
{
    public static class ChannelConstructorGenerator
    {
        public static string Generate(string className, string namespaceName,
            HashSet<string> usings,
            ImmutableArray<(string Name, string Attribute, IPropertySymbol Property)> orderedEdiItems,
            GeneratorItem generatorItem)
        {
            var cw = new CodeWriter();

            cw.AppendLine("#nullable enable");
            
            cw.AddUsing("System.Diagnostics.CodeAnalysis");
            cw.AddUsing("System.Threading.Channels");
            foreach (var @using in usings) cw.AddUsing(@using);

            cw.AppendLine();
            using (var ns = cw.StartNamespace(namespaceName))
            {
                using (var cs = cw.StartClass(className))
                {
                    cw.AppendLine("""
                                  [SuppressMessage("CodeQuality", "IDE0060:Remove unused parameter", Justification = "Empty constructor is required")]
                                  """);
                    using (var con = cw.StartConstructor(className))
                    {
                    }

                    cw.AppendLine();

                    using (var con = cw.StartConstructor(className,
                               arguments:
                               [
                                   "ChannelReader<ISegment> segmentReader", $"{generatorItem.Parent}? parent = null"
                               ]))
                    {
                        cw.AppendLine("var loop = InitializeAsync(segmentReader, parent).GetAwaiter().GetResult();");
                        foreach (var item in orderedEdiItems) cw.AppendLine($"{item.Name} = loop.{item.Name};");
                    }

                    cw.AppendLine();

                    using (var con = cw.AppendBlock(
                               $"public static Task<{className}> InitializeAsync(ChannelReader<ISegment> segmentReader, ILoop? parent)"))
                    {
                        using (var _ = cw.AddIf("parent is null"))
                        {
                            cw.AppendLine($"return InitializeAsync(segmentReader, ({generatorItem.Parent}?) null);");
                        }

                        cw.AppendLine();

                        using (var _ = cw.AddIf($"parent is not {generatorItem.Parent} typedParent"))
                        {
                            cw.AppendLine(
                                $"""throw new ArgumentException($"Parent must be of type {generatorItem.Parent}");""");
                        }

                        cw.AppendLine();

                        cw.AppendLine("return InitializeAsync(segmentReader, typedParent);");
                    }

                    cw.AppendLine();
                    using (var con = cw.AppendBlock(
                               $"public static async Task<{className}> InitializeAsync(ChannelReader<ISegment> segmentReader, {generatorItem.Parent}? parent)"))
                    {
                        cw.AppendLine($"var loop = new {className}();");
                        cw.AppendLine();

                        if (className != generatorItem.Parent)
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
                    $"loop.{item.Name} = await SegmentLoopFactory<{item.Property.Type}, {className}>.CreateAsync(segmentReader, loop);");
        }

        private static void GenerateBody(string className,
            ImmutableArray<(string Name, string Attribute, IPropertySymbol Property)> items, CodeWriter cw)
        {
            using (_ = cw.AddWhile("await segmentReader.WaitToReadAsync()"))
            {
                foreach (var (name, attribute, property) in items)
                {
                    var typeName = ((INamedTypeSymbol)property.Type).TypeArguments is { Length: > 0 } named
                        ? named[0]
                        : property.Type;

                    using var i = cw.AddIf($"await ISegmentIdentifier<{typeName}>.MatchesAsync(segmentReader)");

                    cw.AppendLine(attribute switch
                    {
                        SegmentAttribute or Segment =>
                            $"loop.{name} = await SegmentLoopFactory<{typeName}, {className}>.CreateAsync(segmentReader, loop);",
                        SegmentListAttribute or SegmentList =>
                            $"loop.{name}.Add(await SegmentLoopFactory<{typeName}, {className}>.CreateAsync(segmentReader, loop));",
                        LoopAttribute or Loop =>
                            $"loop.{name} = await {typeName}.InitializeAsync(segmentReader, loop);",
                        LoopListAttribute or LoopList =>
                            $"loop.{name}.Add(await {typeName}.InitializeAsync(segmentReader, loop));",
                        OptionalSegmentFooter or OptionalSegmentFooterAttribute =>
                            $"loop.{name} = SegmentLoopFactory<{typeName}, {className}>.Create(segments, this);",
                        _ => string.Empty
                    });

                    cw.AppendLine("continue;");
                }

                cw.AppendLine("break;");
            }
        }
    }
}

/*


       private async Task InitializeAsync(ChannelReader<ISegment> segmentReader, TransactionSet? parent)
       {
           ST = await SegmentLoopFactory<TS_ST, TransactionSet>.CreateAsync(segmentReader, this);

           while (await segmentReader.WaitToReadAsync())
           {
               if (await ISegmentIdentifier<TS_DTP>.MatchesAsync(segmentReader))
               {
                   DTP = await SegmentLoopFactory<TS_DTP, TransactionSet>.CreateAsync(segmentReader, this);
                   continue;
               }
               if (await ISegmentIdentifier<TS_REF>.MatchesAsync(segmentReader))
               {
                   REFs.Add(await SegmentLoopFactory<TS_REF, TransactionSet>.CreateAsync(segmentReader, this));
                   continue;
               }
               if (await ISegmentIdentifier<Loop2000>.MatchesAsync(segmentReader))
               {
                   Loop2000 = new Loop2000(segmentReader, this);
                   continue;
               }
               if (await ISegmentIdentifier<Loop2100>.MatchesAsync(segmentReader))
               {
                   Loop2100s.Add(new Loop2100(segmentReader, this));
                   continue;
               }
               break;
           }

           SE = await SegmentLoopFactory<TS_SE, TransactionSet>.CreateAsync(segmentReader, this);
       }
   }
   */