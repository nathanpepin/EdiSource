namespace EdiSource.Domain.Standard.Segments;

public abstract class SE<T> : Segment, IEdi<T>, IRefresh
    where T : ITransactionSet<T>, IEdi<FunctionalGroup>, ILoopInitialize<FunctionalGroup, T>, ISegmentIdentifier<T>
{
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
        get => Parent is not ITransactionSet<T> ts
            ? GetCompositeElement(2)
            : ts.GetTransactionSetControlNumber()
                .Do(x => SetCompositeElement(x, 2));
        set => SetCompositeElement(value, 2);
    }

    public abstract T? Parent { get; set; }

    public void Refresh()
    {
        _ = E01NumberOfIncludedSegments;
        _ = E02TransactionSetControlNumber;
    }
}