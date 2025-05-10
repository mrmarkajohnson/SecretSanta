using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Web.TagHelpers;

[HtmlTargetElement("readonly-checkbox", TagStructure = TagStructure.WithoutEndTag)]
public class ReadonlyCheckboxTagHelper : TagHelper
{
    [HtmlAttributeName("for")]    
    public bool For { get; set; }

    public ReadonlyCheckboxTagHelper()
    {
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "input";
        output.Attributes.Add("type", "checkbox");

        if (For)
        {
            output.Attributes.Add("checked", true);
        }

        output.Attributes.Add("readonly", true);
    }
}
