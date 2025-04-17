using Global.Validation;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

public sealed class CustomValidationAttributeAdapterProvider
    : ValidationAttributeAdapterProvider, IValidationAttributeAdapterProvider
{
    public CustomValidationAttributeAdapterProvider() { }

    IAttributeAdapter IValidationAttributeAdapterProvider.GetAttributeAdapter(
        ValidationAttribute attribute,
        IStringLocalizer stringLocalizer)
    {
        IAttributeAdapter adapter = base.GetAttributeAdapter(attribute, stringLocalizer);
        var attributeType = attribute.GetType();

        var matchedLink = ValidationMessages.AttributeMessageLinks.Find(x => attributeType == x.DataAttribute?.UnderlyingSystemType);
        if (matchedLink != null)
        {
            attribute.ErrorMessage = matchedLink.ErrorMessage;
        }

        return adapter;
    }
}