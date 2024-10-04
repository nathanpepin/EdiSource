using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace EdiSource.Generator;

public partial class EdiItemsIncrementalGenerator
{
    private static string EdiElementSourceCode(string className, string namespaceName,
        ImmutableArray<(string Name, string Attribute, IPropertySymbol PropertySymbol)> orderedEdiItems)
    {
        var cw = new CodeWriter();

        cw.AddUsing("System.Collections.Generic");
        cw.AddUsing("EdiSource.Domain.Identifiers");
        cw.AppendLine();
        using (var ns = cw.StartNamespace(namespaceName))
        {
            using (var cl = cw.StartClass(className))
            {
                var names = orderedEdiItems.Select(x => x.Name).ToArray();
                cw.AddCalcProperty("EdiItems", "List<IEdi?>", $$"""new List<IEdi?> { {{string.Join(", ", names)}} }""");
            }
        }

        return cw.ToString();
    }

    private static string ImplementationSourceCode(string className, string namespaceName, string parent, string id)
    {
        var cw = new CodeWriter();

        cw.AddUsing("System.Collections.Generic");
        cw.AddUsing("EdiSource.Domain.Identifiers");
        cw.AppendLine();
        using (var ns = cw.StartNamespace(namespaceName))
        {
            string[] implementations =
                [$"ILoop<{parent}>", $"ISegmentIdentifier<{className}>", $"ISegmentIdentifier<{id}>"];
            using (var cl = cw.StartClass(className, implementations: implementations))
            {
                cw.AppendLine("ILoop? ILoop.Parent => Parent;");
                cw.AppendLine("public TransactionSet? Parent { get; set; } = null;");
                cw.AppendLine($"public static (string Primary, string? Secondary) EdiId => {id}.EdiId;");
            }
        }

        return cw.ToString();
    }
}