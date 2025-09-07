using Application.Areas.Account.BaseModels;
using Global.Abstractions.ViewModels;

namespace Application.Areas.Account.ViewModels;

public class EmailPreferencesVm : UserEmailDetails, IFormVm
{
    public string? ReturnUrl { get; set; }
    public string SubmitButtonText { get; set; } = "Save";
    public string SubmitButtonIcon { get; set; } = "fa fa-save";
    public string? SuccessMessage { get; set; }
}
