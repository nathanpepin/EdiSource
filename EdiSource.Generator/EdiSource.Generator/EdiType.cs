namespace EdiSource.Generator;

public enum EdiType
{
    SegmentHeaderAttribute = 0,
    SegmentAttribute = 1,
    SegmentListAttribute = 2,
    LoopAttribute = 3,
    LoopListAttribute = 4,
    SegmentFooterAttribute = 5,
    Unknown = 99
}