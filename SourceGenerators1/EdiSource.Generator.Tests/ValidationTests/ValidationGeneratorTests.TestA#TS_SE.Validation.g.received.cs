//HintName: TS_SE.Validation.g.cs
namespace EdiSource.Segments
{
    using EdiSource.Domain.Separator;
    using EdiSource.Domain.Segments;
    using EdiSource.Domain.Identifiers;
    using EdiSource.Domain.SourceGeneration;
    using EdiSource.Domain.Loop;
    using EdiSource.Domain.Validation.SourceGeneration;
    using EdiSource.Domain.Validation.Data;
    using System.Linq;
    using System.Collections.Generic;
    using System;

    public partial class TS_SE : ISourceGeneratorValidatable
    {
        public List<IIndirectValidatable> SourceGenValidations => [new ElementLengthAttribute(0, 1, 3, 3), ];
    }
}