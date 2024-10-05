using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Separator;

namespace EdiSource.Domain.Structure;

public sealed class EdiTree
{
    public string Loop { get; set; } = string.Empty;
    public List<string> Segments { get; set; } = [];
    public List<EdiTree> Loops { get; set; } = [];

    public static EdiTree Create<T>(T loop, EdiTree? node = null,
        Separators? separators = null, bool firstIteration = true)
        where T : ILoop
    {
        separators ??= Separators.DefaultSeparators;
        node ??= new EdiTree();

        if (firstIteration) node.Loop = typeof(T).Name;

        loop.EdiAction(x =>
            {
                var text = x.WriteToStringBuilder(separators: separators).ToString();
                node.Segments.Add(text);
            },
            segmentList =>
            {
                foreach (var text in segmentList
                             .Select(segment => segment.WriteToStringBuilder(separators: separators).ToString()))
                    node.Segments.Add(text);
            },
            loopL =>
            {
                var loopNode = new EdiTree
                {
                    Loop = loopL.GetType().Name
                };

                Create(loopL, loopNode, separators, false);
                node.Loops.Add(loopNode);
            },
            loopList =>
            {
                foreach (var loopL in loopList)
                {
                    var loopNode = new EdiTree
                    {
                        Loop = loopL.GetType().Name
                    };

                    Create(loopL, loopNode, separators, false);
                    node.Loops.Add(loopNode);
                }
            });

        return node;
    }
}