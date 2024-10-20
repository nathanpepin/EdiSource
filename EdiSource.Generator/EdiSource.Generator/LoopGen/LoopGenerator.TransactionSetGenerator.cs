using EdiSource.Generator.Helper;

namespace EdiSource.Generator.LoopGen;

public partial class LoopGenerator
{
    public static class TransactionSetGenerator
    {
        public static string Generate(string className, string namespaceName, HashSet<string> usings, string id)
        {
            var cw = new CodeWriter();

            cw.AppendLine("#nullable enable");

            foreach (var @using in usings) cw.AddUsing(@using);

            cw.AppendLine();
            using (cw.StartNamespace(namespaceName))
            {
                using (cw.StartClass(className, [$"ITransactionSet<{className}, {id}>"]))
                {
                    cw.AppendLine("public static TransactionSetDefinition Definition { get; } = id =>");
                    cw.AppendLine("{");
                    cw.IncreaseIndent();
                    using (cw.AddIf(
                               "EdiId.Primary != id.Item1 || (EdiId.Secondary is not null && EdiId.Secondary != id.Item2)"))
                    {
                        cw.AppendLine("return null;");
                    }

                    cw.AppendLine("return (segmentReader, parent) => InitializeAsync(segmentReader, parent)");
                    cw.IncreaseIndent();
                    cw.AppendLineIndent(".ContinueWith(ILoop (x) => x.Result);");
                    cw.DecreaseIndent();
                    cw.DecreaseIndent();
                    cw.AppendLine("};");
                }
            }

            return cw.ToString();
        }
    }
}