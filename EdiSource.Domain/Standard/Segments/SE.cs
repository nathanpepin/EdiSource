using System.Reflection.Emit;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Loop.Extensions;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Segments.Extensions;
using EdiSource.Domain.Standard.Loops;

namespace EdiSource.Domain.Standard.Segments;

public abstract class SE<T> : Segment, IEdi<T>, IRefresh where T : ITransactionSet<T>, IEdi<FunctionalGroup>, ILoopInitialize<FunctionalGroup, T>, ISegmentIdentifier<T>
{
    public abstract T? Parent { get; set; }

    /// <summary>
    ///     If the parent exists then it will count the segments in the
    ///     transaction set, otherwise the value is from the segment itself.
    ///     If a parent does exist, the value will not update.
    /// </summary>
    public int E01NumberOfIncludedSegments
    {
        get
        {
            if (Parent is not ITransactionSet<T> ts)
                return this.GetIntRequired(1);

            var count = ts.GetTransactionSetSegmentCount();
            this.SetInt(count, 1);
            return count;
        }
        set => this.SetInt(value, 1);
    }

    /// <summary>
    ///     If the Parent exists it'll point to the Parent's ST's value,
    ///     otherwise it'll set the value directly on the segment
    /// </summary>
    public string E02TransactionSetControlNumber
    {
        get
        {
            if (Parent is not ITransactionSet<T> ts)
                return GetCompositeElement(2);

            var controlNumber = ts.GetTransactionSetControlNumber();
            SetCompositeElement(controlNumber, 2);
            return controlNumber;
        }
        set => SetCompositeElement(value, 2);
    }

    public void Refresh()
    {
        _ = E01NumberOfIncludedSegments;
        _ = E02TransactionSetControlNumber;
    }
}

public static class ParentHelper
{
    public static T? GetParent<T>(this T segment) where T : IEdi
    {
        
        
        if (segment is IEdi<T> se)
         return se.Parent;

        return default;
    }
}