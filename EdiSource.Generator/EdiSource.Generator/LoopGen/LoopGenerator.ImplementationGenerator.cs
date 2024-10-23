using EdiSource.Generator.Helper;

namespace EdiSource.Generator.LoopGen;

public partial class LoopGenerator
{
    public static class ImplementationGenerator
    {
        public static string Generate(string className, string namespaceName, HashSet<string> usings,
            string parent, string id)
        {
            var cw = new CodeWriter();

            cw.AppendLine("#nullable enable");

            foreach (var @using in usings) cw.AddUsing(@using);

            cw.AppendLine();
            using (cw.StartNamespace(namespaceName))
            {
                string[] implementations =
                [
                    "ILoop", $"ILoop<{parent}>", $"ISegmentIdentifier<{className}>", $"ISegmentIdentifier<{id}>",
                    $"ILoopInitialize<{parent}, {className}>"
                ];
                using (cw.StartClass(className, implementations))
                {
                    // cw.AppendLine("ILoop? ILoop.Parent => Parent;");

                    cw.AppendLine(className == parent
                        ? $$"""public {{parent}}? Parent => null;"""
                        : $$"""public {{parent}}? Parent { get; set; }""");

                    cw.AppendLine($"public static EdiId EdiId => {id}.EdiId;");
                }
            }

            return cw.ToString();
        }
    }
}