using EdiSource.Domain.Identifiers;
using EdiSource.Domain.Validation.Data;

namespace EdiSource.Domain.Validation.Validator;

public static class ValidationHelper
{
    public static void Add<T>(Func<T, IEnumerable<ValidationMessage>?> it) where T : IEdi
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