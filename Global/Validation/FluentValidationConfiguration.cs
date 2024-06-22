using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Resources;
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
            if (memberInfo == null || memberInfo.Name == null)
            {
                return null;
            }

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

        ValidatorOptions.Global.LanguageManager = new CustomLanguageManager();
        ValidatorOptions.Global.MessageFormatterFactory = () => new CustomMessageFormatter();        
    }

    public class CustomMessageFormatter : MessageFormatter
    {
        public override string BuildMessage(string messageTemplate)
        {
            string templateWithoutQuotes = messageTemplate.Replace("'{PropertyName}'", "{PropertyName}");
            string finalMessage = base.BuildMessage(templateWithoutQuotes);            
            return finalMessage;
        }
    }

    public class CustomLanguageManager : LanguageManager
    {
        string[] englishCodes = ["en", "en-GB", "en-US"]; // , "en-AE", "en-BZ", "en-CA", "en-IE", "en-JM", "en-NZ", "en-ZA", "en-TT" ];

        public CustomLanguageManager() // use FluentValidation.Validators to find the classes and get the names
        {
            foreach (var link in ValidationMessages.MessageLinks)
            {
                string errorMessage = link.ConvertMessageForFluentValidation();
                AddEnglishTranslation(link.FluentValidatorName, errorMessage);
            }
        }

        private void AddEnglishTranslation(string validatorName, string errorMessage)
        {
            foreach (string code in englishCodes)
            {
                AddTranslation(code, validatorName, errorMessage);
            }
        }
    }
}
