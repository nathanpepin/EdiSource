global using EdiSource.Domain.SourceGeneration;
global using EdiSource.Domain.Validation.Data;
global using EdiSource.Domain.Validation.Factory;
global using EdiSource.Domain.Standard.Loops;
global using EdiSource.Domain.Loop;
global using EdiSource.Domain.Segments;
global using System.Collections.Generic;
global using static EdiSource.Domain.Identifiers.EdiId;

// Import the transaction set for cross-references
global using EdiSource._270_5010.TransactionSet;
global using EdiSource._270_5010.TransactionSet.Segments;
global using EdiSource.Domain.Segments.Extensions;