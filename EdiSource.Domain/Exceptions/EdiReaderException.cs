namespace EdiSource.Domain.Exceptions;

public sealed class EdiReaderException()
    : Exception("No segments were created. " +
                "This indicates that the EDI string is not valid, likely due to misconfigured separators," +
                "or for envelope reading, an improper ISA was supplied ");