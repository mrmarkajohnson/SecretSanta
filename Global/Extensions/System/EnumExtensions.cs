using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Global.Extensions.System;

public static class EnumExtensions
{
    public static TAttribute? GetAttribute<TAttribute>(this Enum enumValue) where TAttribute : Attribute
    {
        return enumValue.GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<TAttribute>();
    }

    public static string DisplayName(this Enum enumValue)
    {
        var displayNameAttribute = enumValue.GetAttribute<DisplayAttribute>();

        if (displayNameAttribute != null && !string.IsNullOrEmpty(displayNameAttribute.Name))
            return displayNameAttribute.Name;

        var displayAttribute = enumValue.GetAttribute<DisplayAttribute>();
        
        if (displayAttribute != null && !string.IsNullOrEmpty(displayAttribute.Name))
            return displayAttribute.Name;

        return enumValue.ToString();
    }
}
