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
        return ruleBuilder
            .Must((root, x, context) => validator.IsValid(context, x))
            .WithMessage(ConvertMessageForFluentValidation(ValidationMessages.RequiredError));
    }

    public static string ConvertMessageForFluentValidation(string message)
    {
        var matchingLinkMessage = ValidationMessages.MessageLinks.FirstOrDefault(x => x.ErrorMessage == message);
        if (matchingLinkMessage != null)
        {
            return matchingLinkMessage.ConvertMessageForFluentValidation();
        }
        else
        {
            return message.Replace("{0}", "{PropertyName}");
        }
    }
}