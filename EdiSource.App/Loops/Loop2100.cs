using EdiSource.Domain.Loop;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Domain.Validation.Data;
using EdiSource.Segments;

namespace EdiSource.Loops;

[LoopGenerator<_834, Loop2100, Loop2100_NM1>]
public partial class Loop2100 : IValidatable
{
    [SegmentHeader] public Loop2100_NM1 NM1 { get; set; }
    public IEnumerable<ValidationMessage> Validate()
    {
        return [];
    }
}