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
                HandleSegment<T>(validationResult, loopLine, ref segmentLine, segment);
                break;
            case IEnumerable<ISegment> segmentList:
                HandleSegmentList<T>(validationResult, loopLine, ref segmentLine, segmentList);
                break;
            case ILoop loop:
                HandleLoop<T>(validationResult, out loopLine, ref segmentLine, loop);
                break;
            case IEnumerable<ILoop> loopList:
                HanldeLoopList<T>(validationResult, out loopLine, ref segmentLine, loopList);
                break;
        }

        return validationResult;
    }

    private static void HanldeLoopList<T>(EdiValidationResult validationResult, out int loopLine, ref int segmentLine,
        IEnumerable<ILoop> loopList) where T : IEdi
    {
        loopLine = segmentLine;

        foreach (var loop in loopList) HandleLoop<T>(validationResult, out loopLine, ref segmentLine, loop);
    }

    private static void HandleLoop<T>(EdiValidationResult validationResult, out int loopLine, ref int segmentLine,
        ILoop loop) where T : IEdi
    {
        loopLine = segmentLine;

        if (loop is T edi)
            foreach (var userValidation in IUserValidation<T>.UserValidations)
                validationResult.AddRange(userValidation(edi)
                    .UpdateLoopLine(loopLine));

        if (loop is IValidatable v)
            validationResult.AddRange(v
                .Validate()
                .UpdateLoopLine(loopLine));

        if (loop is ISourceGeneratorValidatable v2)
            foreach (var sourceValidation in v2.SourceGenValidations)
                validationResult.AddRange(sourceValidation
                    .Validate(loop)
                    .UpdateLoopLine(loopLine));

        foreach (var item in loop.EdiItems.OfType<IEdi>())
            YieldValidationMessages(item, validationResult, ref loopLine, ref segmentLine);
    }

    private static void HandleSegmentList<T>(EdiValidationResult validationResult, int loopLine, ref int segmentLine,
        IEnumerable<ISegment> segmentList) where T : IEdi
    {
        foreach (var segment in segmentList) HandleSegment<T>(validationResult, loopLine, ref segmentLine, segment);
    }

    private static void HandleSegment<T>(EdiValidationResult validationResult, int loopLine, ref int segmentLine,
        ISegment segment) where T : IEdi
    {
        if (segment is IValidatable v)
            validationResult.AddRange(v
                .Validate()
                .UpdateSegmentLine(segmentLine)
                .UpdateLoopLine(loopLine));

        if (segment is ISourceGeneratorValidatable v2)
            foreach (var sourceValidation in v2.SourceGenValidations)
                validationResult.AddRange(sourceValidation
                    .Validate(segment)
                    .UpdateSegmentLine(segmentLine)
                    .UpdateLoopLine(loopLine));

        if (segment is T t)
            foreach (var userValidation in IUserValidation<T>.UserValidations)
                validationResult.AddRange(userValidation(t)
                    .UpdateSegmentLine(segmentLine)
                    .UpdateLoopLine(loopLine));

        segmentLine++;
    }
}

public static class ValidationHelper
{
    public static void Add<T>(Func<T, IEnumerable<ValidationMessage>> it) where T : IEdi
    {
        IUserValidation<T>.UserValidations.Add(it);
        IUserValidation<IEdi>.UserValidations.Add(x => x is not T t ? [] : it(t));
    }

    public static void Add<T>(List<Func<T, IEnumerable<ValidationMessage>>> items) where T : IEdi
    {
        foreach (var it in items)
        {
            IUserValidation<T>.UserValidations.Add(it);
            IUserValidation<IEdi>.UserValidations.Add(x => x is not T t ? [] : it(t));
        }
    }
    
    public static Func<TR, IEnumerable<ValidationMessage>> ConvertIt<T, TR>(
        this Func<T, IEnumerable<ValidationMessage>> func)
        where T : class
        where TR : class, T
    {
        return func;
    }

    public static List<Func<TR, IEnumerable<ValidationMessage>>> ConvertMultiple<T, TR>(
        this IEnumerable<Func<T, IEnumerable<ValidationMessage>>> func)
        where T : class
        where TR : class, T
    {
        List<Func<TR, IEnumerable<ValidationMessage>>> result = [];
        result.AddRange(func.Select(item => item.ConvertIt<T, TR>()));
        return result;
    }

    public static Func<TR, IEnumerable<ValidationMessage>> ConvertValidatable<T, TR>(
        this T it)
        where T : class, IValidatable
        where TR : class, T
    {
        return _ => it.Validate();
    }
}