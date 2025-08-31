using FluentValidation.Results;
using Global.Validation;
using Global.Validation.CustomValidators;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

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
            .Must((root, x, context) => (allowEmpty && IsEmpty(x))
                || ((TEnumerable?)list.DynamicInvoke(root))?.ToList().Contains(x) == true)
            .WithMessage(ConvertMessageForFluentValidation(ValidationMessages.NotInDropDownError));
    }

    public static IRuleBuilderOptions<T, TProperty> IsInDropDownList<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, 
        Func<T, IEnumerable<SelectListItem>> list, bool allowEmpty)
    {
        return ruleBuilder
            .Must((root, x, context) => (allowEmpty && IsEmpty(x))
                || ((IEnumerable<SelectListItem>?)list.DynamicInvoke(root))?.Any(y => y.Value == x.ToString()) == true)
            .WithMessage(ConvertMessageForFluentValidation(ValidationMessages.NotInDropDownError));
    }

    private static bool IsEmpty<TProperty>(TProperty property)
    {
        if (property == null || Equals(property, default(TProperty)))
            return true;

        if (property is string xString)
            return string.IsNullOrWhiteSpace(xString);

        return false;
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

    public static void AddWarning(this ValidationResult result, string message, string? propertyName = null)
    {
        result.Errors.Add(new ValidationFailure(propertyName ?? "", message) { Severity = Severity.Warning });
    }

    public static void AddError(this ValidationResult result, string message, string? propertyName = null)
    {
        result.Errors.Add(new ValidationFailure(propertyName ?? "", message));
    }
}