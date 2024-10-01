using System.Text;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Seperator;

namespace EdiSource.Domain.Loop;

public interface ILoop
{
    ILoop? Parent { get; }
    IEnumerable<ISegment> YieldChildSegments();
    IEnumerable<ILoop> YieldChildLoops();
}

public interface ILoop<out TParent> : ILoop where TParent : ILoop
{
    new TParent? Parent { get; }
}