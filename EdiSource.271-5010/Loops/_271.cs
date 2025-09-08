namespace EdiSource._271_5010.Loops;


[LoopGenerator<FunctionalGroup, _271, TS271_ST>]
public sealed partial class _271 : IValidatable, ITransactionSet<_271>
{
    [SegmentHeader] public TS271_ST ST { get; set; } = null!;

    [Segment] public TS271_BHT BHT { get; set; } = null!;

    [Loop] public Loop2000A Loop2000A { get; set; } = null!;

    [Loop] public Loop2000B? Loop2000B { get; set; }

    [LoopList] public LoopList<Loop2000C> Loop2000Cs { get; set; } = [];

    [LoopList] public LoopList<Loop2000D> Loop2000Ds { get; set; } = [];

    [SegmentFooter] public TS271_SE SE { get; set; } = null!;

    public static TransactionSetDefinition Definition { get; } =
        TransactionSetDefinitionsFactory<_271>.CreateDefinition();

    public IEnumerable<ValidationMessage> Validate()
    {
        // Basic validation logic - check for required elements
        if (Loop2000A == null!)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "Information Source Level Loop (2000A) is required");

        if (Loop2000Cs.Count == 0)
            yield return ValidationFactory.Create(this, ValidationSeverity.Warning, "At least one Subscriber Level Loop (2000C) is recommended");

        // Validate BHT segment
        if (BHT == null!)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "Beginning of Hierarchical Transaction (BHT) is required");
    }
}
