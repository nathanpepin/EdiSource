namespace EdiSource.Domain.Exceptions;

public sealed class EdiReaderException()
    : Exception("No segments were created. This indicates that the EDI string is not valid, likely due to misconfigured separators,or for envelope reading, an improper ISA was supplied ");

public sealed class EdiSegmentReaderException(string segment, int index, Exception innerException)
    : Exception($"There was an unexpected error reading at index {index}: {segment}", innerException);

public sealed class EmptySegmentException(char segmentSeparator, int index)
    : Exception($"There was an unexpected error reading at index {index} because the segment was empty. " +
                $"Ensure there no double '{segmentSeparator}' terminators on a segments.");