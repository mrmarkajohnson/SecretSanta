using FluentValidation.Internal;
using Global.Validation;
using Global.Validation.CustomValidators;
using System.Linq.Expressions;
using System.Xml.Linq;

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

    public static IRuleBuilderOptions<T, TProperty> IsInDropDownList<T, TProperty, TEnumerable>(this IRuleBuilder<T, TProperty> ruleBuilder, Func<T, TEnumerable> list, bool allowEmpty) 
        where TEnumerable : IEnumerable<TProperty>
    {
        return ruleBuilder
            .Must((root, x, context) => (allowEmpty && (x == null || Equals(x, default(TProperty)) || x is string xString && xString == ""))
                || ((TEnumerable?)list.DynamicInvoke(root))?.ToList().Contains(x) == true) 
            .WithMessage(ConvertMessageForFluentValidation(ValidationMessages.NotInDropDownError));
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