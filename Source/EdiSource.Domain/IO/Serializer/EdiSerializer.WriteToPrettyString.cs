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

            loop.EdiAction(segment =>
                {
                    if (segment is IRefresh refresh)
                        refresh.Refresh();

                    var text = segment.WriteToStringBuilder(separators: separators).ToString();
                    loopPrinter.AppendLine(text);
                },
                segmentList =>
                {
                    foreach (var text in segmentList
                                 .Select(segment =>
                                 {
                                     if (segment is IRefresh refresh)
                                         refresh.Refresh();

                                     return segment.WriteToStringBuilder(separators: separators).ToString();
                                 }))
                        loopPrinter.AppendLine(text);
                },
                loopL =>
                {
                    if (loopL is IRefresh refresh)
                        refresh.Refresh();

                    var loopText = loopL.GetType().Name;
                    using var d = loopPrinter.AppendLoop(loopText);
                    PrettyPrintToStringBuilder(loopL, loopPrinter, separators, false);
                },
                loopList =>
                {
                    foreach (var loopL in loopList)
                    {
                        if (loopL is IRefresh refresh)
                            refresh.Refresh();

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