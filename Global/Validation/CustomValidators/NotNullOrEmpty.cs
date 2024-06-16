using FluentValidation;
using FluentValidation.Validators;
using System.Collections;

namespace Global.Validation.CustomValidators;

/// <summary>
/// Needed for nullable numeric types, but can also be used for strings
/// </summary>
public class NotNullOrEmptyValidator<T, TProperty> : PropertyValidator<T, TProperty>, INotNullValidator, INotEmptyValidator
{
    public override string Name => "NotNullOrEmptyValidator";

    public override bool IsValid(ValidationContext<T> context, TProperty value)
    {
        return IsValid(value);
    }

    public bool IsValid(TProperty value)
    {
        if (value == null || Equals(value, default(T)))
        {
            return false;
        }

        if (value is string s && string.IsNullOrWhiteSpace(s))
        {
            return false;
        }

        if (value is ICollection col && col.Count == 0)
        {
            return false;
        }

        if (value is IEnumerable e && IsEmpty(e))
        {
            return false;
        }

        return !EqualityComparer<TProperty>.Default.Equals(value, default);
    }

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return Localized(errorCode, Name);
    }

    private static bool IsEmpty(IEnumerable enumerable)
    {
        var enumerator = enumerable.GetEnumerator();

        using (enumerator as IDisposable)
        {
            return !enumerator.MoveNext();
        }
    }
}