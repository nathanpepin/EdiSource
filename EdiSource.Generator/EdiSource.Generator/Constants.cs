using System.Collections.Immutable;

namespace EdiSource.Generator;

public static class Constants
{
    //Attribute
    public const string LoopGeneratorAttribute = "LoopGeneratorAttribute";
    public const string SegmentHeaderAttribute = "SegmentHeaderAttribute";
    public const string SegmentFooterAttribute = "SegmentFooterAttribute";
    public const string OptionalSegmentFooterAttribute = "OptionalSegmentFooterAttribute";
    public const string SegmentAttribute = "SegmentAttribute";
    public const string SegmentListAttribute = "SegmentListAttribute";
    public const string LoopAttribute = "LoopAttribute";
    public const string LoopListAttribute = "LoopListAttribute";

    //Non-attribute
    public const string LoopGenerator = "LoopGenerator";
    public const string SegmentHeader = "SegmentHeader";
    public const string SegmentFooter = "SegmentFooter";
    public const string OptionalSegmentFooter = "OptionalSegmentFooter";
    public const string Segment = "Segment";
    public const string SegmentList = "SegmentList";
    public const string Loop = "Loop";
    public const string LoopList = "LoopList";

    public static class LoopAggregation
    {
        public static readonly ImmutableArray<string> LoopGeneratorNames = [LoopGenerator, LoopGeneratorAttribute];
        public static readonly ImmutableArray<string> SegmentHeaderNames = [SegmentHeader, SegmentHeaderAttribute];
        public static readonly ImmutableArray<string> SegmentFooterNames = [SegmentFooter, SegmentFooterAttribute];
        public static readonly ImmutableArray<string> OptionalSegmentFooterNames = [OptionalSegmentFooter, OptionalSegmentFooterAttribute];
        public static readonly ImmutableArray<string> SegmentNames = [Segment, SegmentAttribute];
        public static readonly ImmutableArray<string> SegmentListNames = [SegmentList, SegmentListAttribute];
        public static readonly ImmutableArray<string> LoopNames = [Loop, LoopAttribute];
        public static readonly ImmutableArray<string> LoopListNames = [LoopList, LoopListAttribute];


        public static readonly ImmutableArray<string> Header = SegmentHeaderNames;
        public static readonly ImmutableArray<string> Body = [..SegmentNames, ..SegmentListNames, ..LoopNames, ..LoopListNames, ..OptionalSegmentFooterNames];
        public static readonly ImmutableArray<string> Footer = SegmentFooterNames;
    }
}