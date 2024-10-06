using EdiSource.Domain.Helper.PrettyPrinting;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Loop.Extensions;
using EdiSource.Domain.Segments.Extensions;
using EdiSource.Domain.Separator;

namespace EdiSource.Domain.IO.Serializer;

public sealed partial class EdiSerializer
{
    public string WriteToPrettyString(ILoop loop, Separators? separators = null)
    {
        return PrettyPrintToStringBuilder(loop, separators: separators).ToString();

        static LoopPrinter PrettyPrintToStringBuilder(ILoop loop, LoopPrinter? loopPrinter = null,
            Separators? separators = null, bool firstIteration = true)
        {
            separators ??= Separators.DefaultSeparators;
            loopPrinter ??= new LoopPrinter();

            var loopHeader = firstIteration
                ? loopPrinter.AppendLoop(loop.GetType().Name)
                : null;

            loop.EdiAction(x =>
                {
                    var text = x.WriteToStringBuilder(separators: separators).ToString();
                    loopPrinter.AppendLine(text);
                },
                segmentList =>
                {
                    foreach (var text in segmentList
                                 .Select(segment => segment.WriteToStringBuilder(separators: separators).ToString()))
                        loopPrinter.AppendLine(text);
                },
                loopL =>
                {
                    var loopText = loopL.GetType().Name;
                    using var d = loopPrinter.AppendLoop(loopText);
                    PrettyPrintToStringBuilder(loopL, loopPrinter, separators, false);
                },
                loopList =>
                {
                    foreach (var loopL in loopList)
                    {
                        var loopText = loopL.GetType().Name;
                        using var d = loopPrinter.AppendLoop(loopText);
                        PrettyPrintToStringBuilder(loopL, loopPrinter, separators, false);
                    }
                });

            loopHeader?.Dispose();

            return loopPrinter;
        }
    }
}