namespace EdiSource.Generator.LoopGen.Data;

internal static class LoopSourceGenAttributes
{
    public const string LoopAttribute =
        """
        namespace EdiSource.Domain.SourceGeneration;

        /// <summary>
        ///     With a loop with the LoopGenerator attribute,
        ///     mark a loop with this attribute to enable source generation
        /// </summary>
        [AttributeUsage(AttributeTargets.Property)]
        public sealed class LoopAttribute : Attribute;
        """;

    public const string LoopGeneratorAttribute =
        """
        using EdiSource.Domain.Identifiers;
        using EdiSource.Domain.Loop;
        using EdiSource.Domain.Segments;

        namespace EdiSource.Domain.SourceGeneration;

        #pragma warning disable CS9113 // Parameter is unread.

        /// <summary>
        ///     Enables source generation to occur on a loop.
        ///     Generics are needed for the source generation.
        /// </summary>
        [AttributeUsage(AttributeTargets.Class)]
        public sealed class LoopGeneratorAttribute<TParent, TSelf, TId>(bool isTransactionSet = false) : Attribute
        #pragma warning restore CS9113 // Parameter is unread.
            where TParent : ILoop
            where TSelf : ILoop
            where TId : ISegment, ISegment<TSelf>, ISegmentIdentifier<TId>, IEdi;
        """;

    public const string LoopListAttribute =
        """
        namespace EdiSource.Domain.SourceGeneration;

        /// <summary>
        ///     With a loop with the LoopGenerator attribute,
        ///     mark a loop list with this attribute to enable source generation
        /// </summary>
        [AttributeUsage(AttributeTargets.Property)]
        public sealed class LoopListAttribute : Attribute;
        """;

    public const string OptionalSegmentFooterAttribute =
        """
        namespace EdiSource.Domain.SourceGeneration;

        /// <summary>
        ///     With a loop with the LoopGenerator attribute,
        ///     mark a segment footer with this attribute to enable source generation.
        ///     When the loop constructor finds this element, it will break out the loop.
        /// </summary>
        [AttributeUsage(AttributeTargets.Property)]
        public sealed class OptionalSegmentFooterAttribute : Attribute;
        """;

    public const string SegmentAttribute =
        """
        namespace EdiSource.Domain.SourceGeneration;

        /// <summary>
        ///     With a loop with the LoopGenerator attribute,
        ///     mark a segment with this attribute to enable source generation
        /// </summary>
        [AttributeUsage(AttributeTargets.Property)]
        public sealed class SegmentAttribute : Attribute;
        """;

    public const string SegmentFooterAttribute =
        """
        namespace EdiSource.Domain.SourceGeneration;

        /// <summary>
        ///     With a loop with the LoopGenerator attribute,
        ///     mark a loop with this attribute to enable source generation.
        ///     A segment footer is required and will be expected at the end.
        ///     For instance, an IEA is the footer of and ISA.
        /// </summary>
        [AttributeUsage(AttributeTargets.Property)]
        public sealed class SegmentFooterAttribute : Attribute;
        """;

    public const string SegmentGeneratorAttributes =
        """
        using EdiSource.Domain.Loop;
        using EdiSource.Domain.Segments;

        namespace EdiSource.Domain.SourceGeneration;
        #pragma warning disable CS9113 // Parameter is unread.

        /// <summary>
        ///     Mark a segment class with this attribute to enable source generation.
        /// </summary>
        [AttributeUsage(AttributeTargets.Class)]
        public sealed class SegmentGenerator<TParent>(params string?[] args) : Attribute
            where TParent : ILoop;

        public sealed class SegmentGenerator<TParent, TBase>(params string[] args) : Attribute
            where TParent : ILoop
            where TBase : Segment;
        """;

    public const string SegmentHeaderAttribute =
        """
        namespace EdiSource.Domain.SourceGeneration;

        /// <summary>
        ///     With a loop with the LoopGenerator attribute,
        ///     mark a segment header with this attribute to enable source generation.
        ///     A segment header is required and is expected to begin a loop.
        /// </summary>
        [AttributeUsage(AttributeTargets.Property)]
        public sealed class SegmentHeaderAttribute : Attribute;
        """;

    public const string SegmentListAttribute =
        """
        namespace EdiSource.Domain.SourceGeneration;

        /// <summary>
        ///     With a loop with the LoopGenerator attribute,
        ///     mark a segment list with this attribute to enable source generation
        /// </summary>
        [AttributeUsage(AttributeTargets.Property)]
        public sealed class SegmentListAttribute : Attribute;
        """;
}