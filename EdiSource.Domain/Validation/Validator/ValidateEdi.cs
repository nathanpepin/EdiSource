using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Loop;
using EdiSource.Domain.Segments;
using EdiSource.Domain.Validation.Data;

namespace EdiSource.Domain.Validation.Validator;

public sealed class ValidateEdi : IValidateEdi
{
    /// <summary>
    ///     Validates an IEdi item
    /// </summary>
    /// <param name="ediItem"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public EdiValidationResult Validate<T>(T ediItem) where T : IEdi
    {
        var loopLine = 0;
        var segmentLine = 0;
        return YieldValidationMessages(ediItem, null, ref loopLine, ref segmentLine);
    }

    private static EdiValidationResult YieldValidationMessages<T>(T ediItem, EdiValidationResult? validationResult,
        ref int loopLine, ref int segmentLine) where T : IEdi
    {
        validationResult ??= new EdiValidationResult();

        switch (ediItem)
        {
            case null: return validationResult;
            case ISegment segment:
                segmentLine++;

                if (segment is IValidatable validatable)
                {
                    var updateSegmentLine = validatable
                        .Validate()
                        .UpdateSegmentLine(segmentLine)
                        .UpdateLoopLine(loopLine);
                    validationResult.AddRange(updateSegmentLine);
                }


                break;
            case IEnumerable<ISegment> segmentList:
                foreach (var segment in segmentList)
                {
                    segmentLine++;

                    if (segment is not IValidatable validatable2) continue;

                    var updateSegmentLine = validatable2
                        .Validate()
                        .UpdateSegmentLine(segmentLine)
                        .UpdateLoopLine(loopLine);
                    validationResult.AddRange(updateSegmentLine);
                }

                break;
            case ILoop loop:
                loopLine = segmentLine;

                if (loop is IValidatable validatable3) validationResult.AddRange(validatable3.Validate());

                foreach (var item in loop.EdiItems.OfType<IEdi>())
                    YieldValidationMessages(item, validationResult, ref loopLine, ref segmentLine);

                break;
            case IEnumerable<ILoop> loopList:
                foreach (var loop in loopList)
                {
                    loopLine = segmentLine;

                    if (loop is IValidatable validatable4) validationResult.AddRange(validatable4.Validate());

                    YieldValidationMessages(loop, validationResult, ref loopLine, ref segmentLine);
                }

                break;
        }

        return validationResult;
    }
}