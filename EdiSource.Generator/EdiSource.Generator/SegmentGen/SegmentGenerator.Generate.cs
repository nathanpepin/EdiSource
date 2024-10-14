using EdiSource.Generator.Helper;

namespace EdiSource.Generator.SegmentGen;

public partial class SegmentGenerator
{
    private static string Generate(string className, string namespaceName, HashSet<string> usings, string parent,
        string primaryId, string? secondaryId, string? subType)
    {
        var cw = new CodeWriter();

        cw.AppendLine("#nullable enable");

        foreach (var @using in usings) cw.AddUsing(@using);

        cw.AppendLine();
        using (cw.StartNamespace(namespaceName))
        {
            ReadOnlySpan<string> implementations =
                [subType ?? "Segment", $"ISegment<{parent}>", $"ISegmentIdentifier<{className}>"];

            using (cw.StartClass(className, implementations))
            {
                cw.AppendLine($"new public {parent}? Parent {{ get; set; }}");
                cw.AppendLine(
                    $"public static (string Primary, string? Secondary) EdiId => ({primaryId}, {secondaryId});");
            }
        }

        return cw.ToString();
    }
}