using System.Collections.Immutable;
using EdiSource.Generator.Helper;

namespace EdiSource.Generator.EdiElementGenerator;

public partial class EdiItemsIncrementalGenerator
{
    public static class ImplementationGenerator
    {
        public static string Generate(string className, string namespaceName, ImmutableArray<string> usings,
            string parent, string id)
        {
            var cw = new CodeWriter();

            cw.AppendLine("#nullable enable");

            cw.AddUsing("System.Collections.Generic");
            foreach (var @using in usings)
            {
                cw.AddUsing(@using);
            }

            cw.AppendLine();
            using (var ns = cw.StartNamespace(namespaceName))
            {
                string[] implementations =
                [
                    $"ILoop<{parent}>", $"ISegmentIdentifier<{className}>", $"ISegmentIdentifier<{id}>",
                    $"ILoopInitialize<{parent}, {className}>"
                ];
                using (var cl = cw.StartClass(className, implementations: implementations))
                {
                    cw.AppendLine("ILoop? ILoop.Parent => Parent;");

                    cw.AppendLine(className == parent
                        ? $$"""public {{parent}}? Parent { get => null; set {} }"""
                        : $$"""public {{parent}}? Parent { get; set; }""");

                    cw.AppendLine($"public static (string Primary, string? Secondary) EdiId => {id}.EdiId;");
                }
            }

            return cw.ToString();
        }
    }
}