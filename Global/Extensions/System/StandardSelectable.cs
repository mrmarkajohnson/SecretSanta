using Microsoft.AspNetCore.Mvc.Rendering;

namespace Global.Extensions.System;

public sealed class StandardSelectable : SelectListItem
{
    public StandardSelectable(int key, string displayText)
    {
        Key = key;
        DisplayText = displayText;
    }

    public int Key
    {
        get => int.Parse(Value);
        set => Value = value.ToString();
    }

    public string DisplayText
    {
        get => Text;
        set => Text = value;
    }
}
