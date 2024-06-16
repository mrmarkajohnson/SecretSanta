using Global.Validation;
using Global.Validation.CustomValidators;

namespace FluentValidation;

public static class ValidationExtensions
{
    /// <summary>
    /// Needed for nullable numeric types, but can also be used for strings
    /// </summary>
    public static IRuleBuilderOptions<T, TProperty> NotNullOrEmpty<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
    {
        var validator = new NotNullOrEmptyValidator<T, TProperty>();
        return ruleBuilder.Must(x => validator.IsValid(x)).WithMessage(ValidationMessages.RequiredError);
    }
}