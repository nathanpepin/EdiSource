using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Segments.Extensions;
using EdiSource.Domain.Standard.Loops;

namespace EdiSource.Domain.Standard.Segments;

public sealed class GE : Segment, IEdi<FunctionalGroup>, ISegmentIdentifier<GE>, IRefresh
{
    public int E01NumberOfTransactionSets
    {
        get
        {
            if (Parent is not { } fg)
                return this.GetIntRequired(1);

            var count = fg.TransactionSets.Count;
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
            if (Parent is not null) Parent.GS.E06GroupControlNumber = value;

            SetCompositeElement(value, 2);
        }
    }

    public FunctionalGroup? Parent { get; set; }

    public void Refresh()
    {
        _ = E01NumberOfTransactionSets;
        _ = E02GroupControlNumber;
    }

    public static EdiId EdiId { get; } = new("GE");
}