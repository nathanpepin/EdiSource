using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop.Extensions;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Segments.Extensions;
using EdiSource.Domain.Standard.Loops;

namespace EdiSource.Domain.Standard.Segments;

public sealed class GE : Segment, ISegment<FunctionalGroup>, ISegmentIdentifier<GE>, IRefresh
{
    public new FunctionalGroup? Parent { get; }
    public static (string Primary, string? Secondary) EdiId { get; } = ("GE", null);

    public int E01NumberOfTransactionSets
    {
        get
        {
            if (Parent is null)
                return this.GetIntRequired(1);

            var count = Parent.TransactionSets.Count;
            this.SetInt(count, 1);
            return count;
        }
        set
        {
            if (Parent is null)
                this.SetInt(value, 1);
        }
    }

    public string E02GroupControlNumber
    {
        get => Parent is null
            ? GetCompositeElement(2)
            : Parent.GS.E06GroupControlNumber;
        set
        {
            if (Parent is not null)
            {
                Parent.GS.E06GroupControlNumber = value;
            }

            SetCompositeElement(value, 2);
        }
    }

    public void Refresh()
    {
        _ = E01NumberOfTransactionSets;
        _ = E02GroupControlNumber;
    }
}