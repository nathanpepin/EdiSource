using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using EdiSource.Domain.Identifiers;
using EdiSource.Domain.SourceGeneration;
using EdiSource.Domain.Standard.Segments.DTPData;
using EdiSource.Domain.Validation.Data;
using EdiSource.Domain.Validation.Factory;
using EdiSource.Domain.Validation.SourceGeneration;
using EdiSource.Domain.Validation.Validator;
using EdiSource.Loops;
using Spectre.Console;

namespace EdiSource.Segments;

[ElementLength(ValidationSeverity.Critical, 0, 20)]
[SegmentGenerator<_834, DTP>("DTP")]
public partial class TS_DTP;

