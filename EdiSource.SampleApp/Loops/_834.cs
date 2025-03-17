using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Domain.Standard.Loops;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.Factory;

namespace EdiSource.Loops;

// Main 834 Transaction Set
[LoopGenerator<FunctionalGroup, _834, Segments.TS_ST>]
public sealed partial class _834 : IValidatable, ITransactionSet<_834>
{
    [SegmentHeader] public Segments.TS_ST ST { get; set; } = default!;

    [SegmentList] public SegmentList<Segments.TS_REF> REFs { get; set; } = [];

    [Segment] public Segments.TS_DTP? DTP { get; set; }

    [Loop] public Loop2000 Loop2000 { get; set; } = default!;

    [LoopList] public LoopList<Loop2100> Loop2100s { get; set; } = [];

    [SegmentFooter] public Segments.TS_SE SE { get; set; } = default!;

    public IEnumerable<ValidationMessage> Validate()
    {
        // Basic validation logic - check for required elements
        if (Loop2000 == null!)
            yield return ValidationFactory.Create(this, ValidationSeverity.Critical, "Insured loop (2000) is required");

        if (Loop2100s.Count == 0)
            yield return ValidationFactory.Create(this, ValidationSeverity.Warning, "At least one member detail loop (2100) is recommended");
    }
    
    public static TransactionSetDefinition Definition { get; } =
        TransactionSetDefinitionsFactory<_834>.CreateDefinition();
}