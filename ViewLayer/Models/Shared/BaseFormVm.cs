using ViewLayer.Abstractions;

namespace ViewLayer.Models.Shared;

public class BaseFormVm : BasePageVm, IFormVm
{
    public virtual string? ReturnUrl { get; set; }
    public virtual string SubmitButtonText { get; set; } = "Save";
    public virtual string SubmitButtonIcon { get; set; } = "fa-save";
}
