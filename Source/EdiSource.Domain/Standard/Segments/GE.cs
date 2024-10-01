namespace EdiSource.Domain.Standard.Segments;

public sealed class GE : Segment, IEdi<FunctionalGroup>, ISegmentIdentifier<GE>, IRefresh
{
    public int E01NumberOfTransactionSets
    {
        get => Parent is null
            ? this.GetIntRequired(1)
            : Parent.TransactionSets.Count
                .Do(x => this.SetInt(x, 1));
        set => this.SetInt(value, 1);
    }

    public string E02GroupControlNumber
    {
        get => Parent is null
            ? GetCompositeElement(2)
            : Parent.GS.E06GroupControlNumber
                .Do(x => SetCompositeElement(x, 2));
        set => SetCompositeElement(value, 2);
    }

    public FunctionalGroup? Parent { get; set; }

    public void Refresh()
    {
        _ = E01NumberOfTransactionSets;
        _ = E02GroupControlNumber;
    }

    public static EdiId EdiId { get; } = new("GE");
}