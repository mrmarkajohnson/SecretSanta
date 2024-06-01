using Global.Abstractions.Global;

namespace ViewLayer.Models.Shared;

public class BaseFormVm : BasePageVm, IForm
{
    public virtual string? ReturnUrl { get; set; }
    public virtual string SubmitButtonText { get; set; } = "Save";
    public virtual string SubmitButtonIcon { get; set; } = "fa-save";
}
