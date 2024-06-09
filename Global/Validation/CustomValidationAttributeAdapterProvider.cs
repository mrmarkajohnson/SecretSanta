using Global.Validation;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

public class CustomValidationAttributeAdapterProvider
    : ValidationAttributeAdapterProvider, IValidationAttributeAdapterProvider
{
    public CustomValidationAttributeAdapterProvider() { }

    IAttributeAdapter IValidationAttributeAdapterProvider.GetAttributeAdapter(
        ValidationAttribute attribute,
        IStringLocalizer stringLocalizer)
    {
        IAttributeAdapter adapter = base.GetAttributeAdapter(attribute, stringLocalizer);
        var attributeType = attribute.GetType();

        var matchedLink = ValidationMessages.AttributeMessageLinks.FirstOrDefault(x => attributeType == x.DataAttribute?.UnderlyingSystemType);
        if (matchedLink != null)
        {
            attribute.ErrorMessage = matchedLink.ErrorMessage;
        }

        //foreach (var link in ValidationMessages.MessageLinks.Where(x => x.DataAttribute != null).DistinctBy(x => x.DataAttribute).ToList())
        //{            
        //    var linkAttributeType = link.DataAttribute?.UnderlyingSystemType;
        //    if (attributeType == linkAttributeType && attributeType != typeof(ValidationAttribute)) 
        //    {
        //        attribute.ErrorMessage = link.ErrorMessage;
        //    }
        //}

        //if (attribute is RequiredAttribute)
        //{
        //    attribute.ErrorMessage = ValidationMessages.RequiredError;
        //}
        //else if (attribute is EmailAddressAttribute)
        //{
        //    attribute.ErrorMessage = ValidationMessages.EmailError;
        //}
        //else if (attribute is LengthAttribute || attribute is StringLengthAttribute)
        //{
        //    attribute.ErrorMessage = ValidationMessages.LengthError;
        //}
        //else if (attribute is MaxLengthAttribute)
        //{
        //    attribute.ErrorMessage = ValidationMessages.MaxLengthError;
        //}
        //else if (attribute is MinLengthAttribute)
        //{
        //    attribute.ErrorMessage = ValidationMessages.MinLengthError;
        //}

        return adapter;
    }
}