namespace EdiSource.Domain.Exceptions;

/// <summary>
///     Thrown when no segments could be created from the input, typically due to
///     misconfigured separators or an invalid ISA segment.
/// </summary>
public sealed class EdiReaderException()
    : Exception("No segments were created. This indicates that the EDI string is not valid, likely due to misconfigured separators, or for envelope reading, an improper ISA was supplied.");

/// <summary>
///     Thrown when an unexpected error occurs while reading a specific segment at a given index.
/// </summary>
/// <param name="segment">The segment text that caused the error.</param>
/// <param name="index">The position index where the error occurred.</param>
/// <param name="innerException">The underlying exception.</param>
public sealed class EdiSegmentReaderException(string segment, int index, Exception innerException)
    : Exception($"There was an unexpected error reading at index {index}: {segment}", innerException);

/// <summary>
///     Thrown when an empty segment is encountered, typically caused by consecutive segment separators.
/// </summary>
/// <param name="segmentSeparator">The segment separator character.</param>
/// <param name="index">The position index where the empty segment was found.</param>
public sealed class EmptySegmentException(char segmentSeparator, int index)
    : Exception($"There was an unexpected error reading at index {index} because the segment was empty. " +
                $"Ensure there are no double '{segmentSeparator}' terminators on segments.");

/// <summary>
///     Thrown when the ISA segment is malformed or cannot be read.
/// </summary>
/// <param name="reason">A description of why the ISA segment is invalid.</param>
public sealed class InvalidISAException(string reason)
    : Exception($"There was an issue reading the ISA segment: {reason}");