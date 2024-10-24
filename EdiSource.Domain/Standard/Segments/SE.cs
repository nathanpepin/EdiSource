using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Standard.Loops;

namespace EdiSource.Domain.Standard.Segments;

public class Generic_SE : SE, IEdi<GenericTransactionSet>, ISegmentIdentifier<Generic_SE>
{
    public new GenericTransactionSet? Parent { get; set; }
}

public class SE : Segment, ISegmentIdentifier<SE>, IRefresh
{
    public ITransactionSet? Parent { get; set; }

    /// <summary>
    ///     If the parent exists then it will count the segments in the
    ///     transaction set, otherwise the value is from the segment itself.
    ///     If a parent does exist, the value will not update.
    /// </summary>
    public int E01NumberOfIncludedSegments
    {
        get => 0;
        // if (Parent is null)
        // return this.GetIntRequired(1);
        // var count = Parent.YieldChildSegments().Count();
        // this.SetInt(count, 1);
        // return count;
        set
        {
            // if (Parent is null)
            //     this.SetInt(value, 1);
        }
    }

    /// <summary>
    ///     If the Parent exists it'll point to the Parent's ST's value,
    ///     otherwise it'll set the value directly on the segment
    /// </summary>
    public string E02TransactionSetControlNumber
    {
        get => "";
        set { }
    }

    public void Refresh()
    {
        _ = E01NumberOfIncludedSegments;
        _ = E02TransactionSetControlNumber;
    }

    public static EdiId EdiId { get; } = new("SE");
}