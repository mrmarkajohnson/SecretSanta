using Application.Areas.Account.BaseModels;
using Global.Abstractions.Areas.Account;
using System.ComponentModel.DataAnnotations;

namespace ViewLayer.Models.Account;

public class UpdateDetailsVm : SantaUser, IUpdateSantaUser, IForm
{
   [Required]
    [Display(Name = "Password"), DataType(DataType.Password)]
    public string CurrentPassword { get; set; } = string.Empty;

    public bool LockedOut { get; set; }

    public string? ReturnUrl { get; set; }
    public string? SuccessMessage { get; set; }
    public string SubmitButtonText { get; set; } = "Update";
    public string SubmitButtonIcon { get; set; } = "fa-id-card";
}

public class UpdateDetailsVmValidator : SantaUserValidator<UpdateDetailsVm>
{
}
