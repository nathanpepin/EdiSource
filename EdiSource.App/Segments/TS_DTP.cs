using EdiSource.Domain.Elements;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Segments.Extensions;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Loops;

namespace EdiSource.Segments;

[SegmentGenerator<_834>("DTP", null)]
public partial class TS_DTP
{
    public DateOnly? Date
    {
        get => this.GetDateOnly(2);
        set => this.SetDateOnly(value, 2);
    }
}