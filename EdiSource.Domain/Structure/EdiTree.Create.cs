using EdiSource.Domain.Loop;
using EdiSource.Domain.Loop.Extensions;
using EdiSource.Domain.Segments.Extensions;
using EdiSource.Domain.Separator;

namespace EdiSource.Domain.Structure;

public sealed partial class EdiTree
{
    /// <summary>
    ///     Creates an EdiTree from the given loop with specified separators.
    /// </summary>
    /// <typeparam name="T">The type of the loop implementing ILoop interface.</typeparam>
    /// <param name="loop">The loop to be converted into an EdiTree.</param>
    /// <param name="node">Optional EdiTree node to accumulate the data. If null, a new tree node will be created.</param>
    /// <param name="separators">Optional separators to be used in EDI generation. If null, default separators will be used.</param>
    /// <param name="firstIteration">Flag indicating if this is the first call in the recursion chain.</param>
    /// <returns>The constructed EdiTree from the given loop.</returns>
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