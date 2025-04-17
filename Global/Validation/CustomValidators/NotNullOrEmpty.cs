using FluentValidation;
using FluentValidation.Validators;
using Global.Extensions.System;

namespace Global.Validation.CustomValidators;

/// <summary>
/// Needed for nullable numeric types, but can also be used for strings
/// </summary>
public sealed class NotNullOrEmptyValidator<T, TProperty> : PropertyValidator<T, TProperty>, INotNullValidator, INotEmptyValidator
{
    public override string Name => "NotNullOrEmptyValidator";

    public override bool IsValid(ValidationContext<T> context, TProperty value)            
    {
        if (value == null || Equals(value, default(T)))
        {
            return false;
        }

        Type originalType = typeof(T);
        Type nonNullableType = Nullable.GetUnderlyingType(originalType) ?? originalType;

        if (nonNullableType != originalType) 
        {
            if (Equals(value, nonNullableType.GetDefault()))
            {
                return false;
            }             
        }

        return new NotEmptyValidator<T, TProperty>().IsValid(context, value);
    }

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return Localized(errorCode, Name);
    }
}