namespace EdiSource.SampleApp.Loops;

// Main 834 Transaction Set
[LoopGenerator<FunctionalGroup, _834, TS_ST>]
public sealed partial class _834 : IValidatable, ITransactionSet<_834>
{
    [SegmentHeader] public TS_ST ST { get; set; } = default!;

    [SegmentList] public SegmentList<TS_REF> REFs { get; set; } = [];

    [Segment] public TS_DTP? DTP { get; set; }

    [Loop] public Loop2000 Loop2000 { get; set; } = default!;

    [LoopList] public LoopList<Loop2100> Loop2100s { get; set; } = [];

    [SegmentFooter] public TS_SE SE { get; set; } = default!;

    public static TransactionSetDefinition Definition { get; } =
        TransactionSetDefinitionsFactory<_834>.CreateDefinition();

    public IEnumerable<ValidationMessage> Validate()
    {
        // Basic validation logic - check for required elements
        if (Loop2000 == null!)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "Insured loop (2000) is required");

        if (Loop2100s.Count == 0)
            yield return ValidationFactory.Create(this, ValidationSeverity.Warning, "At least one member detail loop (2100) is recommended");
    }
}