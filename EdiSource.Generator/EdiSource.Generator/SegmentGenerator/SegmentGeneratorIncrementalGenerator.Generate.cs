using System;
using System.Collections.Generic;
using EdiSource.Generator.Helper;

namespace EdiSource.Generator.SegmentGenerator;

public partial class SegmentGeneratorIncrementalGenerator
{
    private static string Generate(string className, string namespaceName, HashSet<string> usings, string parent,
        string primaryId, string secondaryId)
    {
        var cw = new CodeWriter();

        cw.AppendLine("#nullable enable");

        cw.AddUsing("System.Collections.Generic");
        cw.AddUsing("EdiSource.Domain.Segments");
        cw.AddUsing("EdiSource.Domain.Identifiers");
        foreach (var @using in usings) cw.AddUsing(@using);

        cw.AppendLine();
        using (cw.StartNamespace(namespaceName))
        {
            ReadOnlySpan<string> implementations =
                ["Segment", $"ISegment<{parent}>", $"ISegmentIdentifier<{className}>"];
            
            using (cw.StartClass(className, implementations: implementations))
            {
                cw.AppendLine($"new public {parent}? Parent {{ get; set; }}");
                cw.AppendLine(
                    $"public static (string Primary, string? Secondary) EdiId => ({primaryId}, {secondaryId});");
            }
        }

        return cw.ToString();
    }
}