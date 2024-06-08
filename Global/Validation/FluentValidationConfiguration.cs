using FluentValidation;
using Global.Abstractions.Extensions;
using System.ComponentModel;
using System.Reflection;

namespace Global.Validation;

public static class FluentValidationConfiguration
{
    public static void SetFluentValidationOptions()
    {
        ValidatorOptions.Global.DisplayNameResolver = (type, memberInfo, expression) =>
        {

            string? displayName = null;
            PropertyInfo? originalProperty = type.GetProperties().FirstOrDefault(x => x.Name == memberInfo.Name);

            if (originalProperty != null)
            {
                displayName = originalProperty.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName
                    ?? originalProperty.GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.GetName();
            }

            displayName ??= memberInfo.GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>()?.GetName();

            return displayName ?? ValidatorOptions.Global.PropertyNameResolver(type, memberInfo, expression).SplitPascalCase();
        };
    }
}
