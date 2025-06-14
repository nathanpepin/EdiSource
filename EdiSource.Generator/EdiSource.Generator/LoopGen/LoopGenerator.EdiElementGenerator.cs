using System.Collections.Immutable;
using EdiSource.Generator.Helper;
using Microsoft.CodeAnalysis;

namespace EdiSource.Generator.LoopGen;

public partial class LoopGenerator
{
    public static class EdiElementGenerator
    {
        public static string Generate(string className, string namespaceName,
            HashSet<string> usings,
            ImmutableArray<(string Name, string Attribute, IPropertySymbol PropertySymbol)> orderedEdiItems)
        {
            var cw = new CodeWriter();

            cw.AppendLine("#nullable enable");

            foreach (var @using in usings) cw.AddUsing(@using);

            cw.AppendLine();
            using (cw.StartNamespace(namespaceName))
            {
                using (cw.StartClass(className, []))
                {
                    var names = orderedEdiItems.Select(x => x.Name).ToArray();
                    cw.AddCalcProperty("EdiItems", "List<IEdi?>",
                        $$"""new List<IEdi?> { {{string.Join(", ", names)}} }""");
                }
            }

            return cw.ToString();
        }
    }
}