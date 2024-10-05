using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.IO.EdiWriter;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Seperator;

namespace EdiSource.Domain.Loop;

public static class LoopExtensions
{
    public static void EdiAction<T>(this T it, Action<ISegment>? segmentAction = null,
        Action<SegmentList<ISegment>>? segmentListAction = null,
        Action<ILoop>? loopAction = null, Action<LoopList<ILoop>>? loopListAction = null)
        where T : ILoop
    {
        foreach (var ediItem in it.EdiItems)
        {
            switch (ediItem)
            {
                case null: continue;
                case ISegment segment:
                    segmentAction?.Invoke(segment);
                    continue;
                case IEnumerable<ISegment> segmentList:
                    segmentListAction?.Invoke([..segmentList]);
                    continue;
                case ILoop loop:
                    loopAction?.Invoke(loop);
                    continue;
                case IEnumerable<ILoop> loopList:
                    loopListAction?.Invoke([..loopList]);
                    continue;
            }
        }
    }

    public static IEnumerable<ISegment> YieldChildSegments<T>(this T it, bool recursive = true)
        where T : ILoop
    {
        List<ISegment> items = [];
        SegmentAction(it, x => items.Add(x), recursive);
        return items;
    }

    public static void SegmentAction<T>(this T it, Action<ISegment> action, bool recursive = true)
        where T : ILoop
    {
        EdiAction(it,
            segmentAction: action,
            segmentListAction: segmentList =>
            {
                foreach (var segment in segmentList)
                {
                    action(segment);
                }
            },
            loopAction: loop =>
            {
                if (!recursive) return;
                foreach (var childSegment in loop.YieldChildSegments())
                {
                    action(childSegment);
                }
            },
            loopListAction: loopList =>
            {
                if (!recursive) return;
                foreach (var childSegment in loopList.SelectMany(x => x.YieldChildSegments()))
                {
                    action(childSegment);
                }
            });
    }

    public static IEnumerable<ILoop> YieldChildLoops<T>(this T it, bool recursive = true)
        where T : ILoop
    {
        List<ILoop> items = [];
        EdiAction(it,
            loopAction: loop =>
            {
                items.Add(loop);

                if (recursive)
                    items.AddRange(loop.YieldChildLoops());
            },
            loopListAction: loopList =>
            {
                items.AddRange(loopList);

                if (recursive)
                    items.AddRange(loopList.SelectMany(x => x.YieldChildLoops()));
            });
        return items;
    }

    public static IEnumerable<T> FindEdiElement<T>(this ILoop it) where T : IEdi
    {
        List<T> output = [];

        EdiAction(it,
            segmentAction: x =>
            {
                if (x is T t)
                    output.Add(t);
            },
            segmentListAction: segmentList =>
            {
                foreach (var segment in segmentList)
                {
                    if (segment is T t)
                        output.Add(t);
                }
            },
            loopAction: loop =>
            {
                if (loop is T t)
                {
                    output.Add(t);
                    return;
                }

                loop.FindEdiElement<T>();
            },
            loopListAction: loopList =>
            {
                foreach (var loop in loopList)
                {
                    var found = false;

                    if (loop is T t)
                    {
                        output.Add(t);
                        found = true;
                    }

                    if (found) return;

                    loop.FindEdiElement<T>();
                }
            });

        return output;
    }

    public static StringBuilder WriteToStringBuilder<T>(this T loop, StringBuilder? stringBuilder = null,
        Separators? separators = null, bool includeNewLine = true)
        where T : ILoop
    {
        separators ??= Separators.DefaultSeparators;
        stringBuilder ??= new StringBuilder();

        var childSegments = loop.YieldChildSegments().ToArray();

        foreach (var segment in loop.YieldChildSegments())
        {
            segment.WriteToStringBuilder(stringBuilder, separators);

            if (includeNewLine)
                stringBuilder.AppendLine();
        }

        return stringBuilder;
    }

    public static async Task WriteToStream<T>(this T loop, Stream stream,
        Separators? separators = null, bool includeNewLine = true, CancellationToken cancellationToken = default)
        where T : ILoop
    {
        separators ??= Separators.DefaultSeparators;

        using var writer = new StreamEdiWriter(stream);

        foreach (var segment in loop.YieldChildSegments())
        {
            var text = segment.WriteToStringBuilder(separators: separators).ToString();
            await writer.WriteTextAsync(text, cancellationToken);

            if (includeNewLine)
                await writer.WriteTextAsync(Environment.NewLine, cancellationToken);
        }
    }

    public static LoopPrinter PrettyPrintToStringBuilder<T>(this T loop, LoopPrinter? loopPrinter = null,
        Separators? separators = null, bool firstIteration = true)
        where T : ILoop
    {
        separators ??= Separators.DefaultSeparators;
        loopPrinter ??= new LoopPrinter();

        var loopHeader = firstIteration
            ? loopPrinter.AppendLoop(typeof(T).Name)
            : null;

        EdiAction(loop,
            segmentAction: x =>
            {
                var text = x.WriteToStringBuilder(separators: separators).ToString();
                loopPrinter.AppendLine(text);
            },
            segmentListAction: segmentList =>
            {
                foreach (var text in segmentList
                             .Select(segment => segment.WriteToStringBuilder(separators: separators).ToString()))
                {
                    loopPrinter.AppendLine(text);
                }
            },
            loopAction: loopL =>
            {
                var loopText = loopL.GetType().Name;
                using var d = loopPrinter.AppendLoop(loopText);
                loopL.PrettyPrintToStringBuilder(loopPrinter, separators, firstIteration: false);
            },
            loopListAction: loopList =>
            {
                foreach (var loopL in loopList)
                {
                    var loopText = loopL.GetType().Name;
                    using var d = loopPrinter.AppendLoop(loopText);
                    loopL.PrettyPrintToStringBuilder(loopPrinter, separators, firstIteration: false);
                }
            });

        loopHeader?.Dispose();

        return loopPrinter;
    }

    public static EdiTree ToTree<T>(this T loop, EdiTree? node = null,
        Separators? separators = null, bool firstIteration = true)
        where T : ILoop
    {
        separators ??= Separators.DefaultSeparators;
        node ??= new EdiTree();

        if (firstIteration)
        {
            node.Name = typeof(T).Name;
        }

        EdiAction(loop,
            segmentAction: x =>
            {
                var text = x.WriteToStringBuilder(separators: separators).ToString();
                node.Children.Add(text);
            },
            segmentListAction: segmentList =>
            {
                foreach (var text in segmentList
                             .Select(segment => segment.WriteToStringBuilder(separators: separators).ToString()))
                {
                    node.Children.Add(text);
                }
            },
            loopAction: loopL =>
            {
                var loopNode = new EdiTree
                {
                    Name = loopL.GetType().Name,
                };

                loopL.ToTree(loopNode, separators, firstIteration: false);
                node.ChildrenTrees.Add(loopNode);
            },
            loopListAction: loopList =>
            {
                foreach (var loopL in loopList)
                {
                    var loopNode = new EdiTree
                    {
                        Name = loopL.GetType().Name,
                    };

                    loopL.ToTree(loopNode, separators, firstIteration: false);
                    node.ChildrenTrees.Add(loopNode);
                }
            });

        return node;
    }

    public static ILoop GetRoot(this ILoop loop, bool avoidCircularReferences = true)
    {
        var iterations = 0;
        const int maxIterations = 1_000;

        while (true)
        {
            if (loop.Parent is null) return loop;
            loop = loop.Parent;

            if (avoidCircularReferences && iterations++ > maxIterations)
                throw new Exception($"Loop has more than {maxIterations} iterations, likely a circular reference.");
        }
    }

    public static T? FindParent<T>(this ILoop loop, bool avoidCircularReferences = true) where T : ILoop
    {
        var iterations = 0;
        const int maxIterations = 1_000;

        while (true)
        {
            if (loop is T foundLoop) return foundLoop;

            if (loop.Parent is null) return default;

            loop = loop.Parent;

            if (avoidCircularReferences && iterations++ > maxIterations)
                throw new Exception($"Loop has more than {maxIterations} iterations, likely a circular reference.");
        }
    }
}

public sealed class LoopPrinter
{
    private readonly StringBuilder _builder = new();
    private int _indentLevel;
    private readonly Stack<string> _disposeStack = new(0);

    private const string Indent =
        "                                                                                                                                ";

    public void AppendLine(string line = "")
    {
        if (!string.IsNullOrEmpty(line))
        {
            _builder.Append(Indent.AsSpan(0, _indentLevel * 4));
            _builder.AppendLine(line);
        }
        else
        {
            _builder.AppendLine();
        }
    }


    public IDisposable AppendLoop(string line = "")
    {
        if (!string.IsNullOrEmpty(line))
        {
            _builder.Append(Indent[..(_indentLevel * 4)]);
            _builder.AppendLine(line);
        }
        else
        {
            _builder.AppendLine();
        }

        return new IndentationBlock(this);
    }

    public IDisposable IndentBlock()
    {
        return new IndentationBlock(this);
    }

    private class IndentationBlock : IDisposable
    {
        private readonly LoopPrinter _writer;

        public IndentationBlock(LoopPrinter writer)
        {
            _writer = writer;
            _writer._indentLevel++;
        }

        public void Dispose()
        {
            _writer._indentLevel--;
        }
    }

    public override string ToString()
    {
        return _builder.ToString();
    }
}

public sealed class EdiTree
{
    public string Name { get; set; } = string.Empty;
    public List<string> Children { get; set; } = [];
    public List<EdiTree> ChildrenTrees { get; set; } = [];
}