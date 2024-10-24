using System.Collections.Immutable;
using EdiSource.Generator.Helper;

namespace EdiSource.Generator.SegmentGen;

public partial class SegmentGenerator
{
    private static string Generate(string className, string namespaceName, HashSet<string> usings, string parent,
        ImmutableArray<string> args, string? subType)
    {
        var cw = new CodeWriter();

        foreach (var @using in usings) cw.AddUsing(@using);

        cw.AppendLine();
        using (cw.StartNamespace(namespaceName))
        {
            ReadOnlySpan<string> implementations =
                [subType ?? "Segment", $"IEdi<{parent}>", $"ISegmentIdentifier<{className}>"];

            using (cw.StartClass(className, implementations))
            {
                cw.AppendLine($"public {parent}? Parent {{ get; set; }}");
                cw.AppendLine(
                    $"public static EdiId EdiId => new ({string.Join(", ", args)});");
            }
        }

        return cw.ToString();
    }
}