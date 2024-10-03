using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace EdiSource.Generator;

public partial class EdiItemsIncrementalGenerator
{
    private static string GenerateSourceCode(string className, string namespaceName,
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
}