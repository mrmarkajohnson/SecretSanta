using FluentValidation;
using Global.Abstractions.Areas.Account;
using Global.Abstractions.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Application.Areas.Account.ViewModels;

public sealed class UpdateDetailsVm : PersonalDetailsBaseVm, IUpdateSantaUser, IFormVm
{
    [Required]
    [Display(Name = "Password"), DataType(DataType.Password)]
    public string CurrentPassword { get; set; } = string.Empty;

    public bool LockedOut { get; set; }

    public override string SubmitButtonText { get; set; } = "Update";
    public override string SubmitButtonIcon { get; set; } = "fa-id-card";
}

public sealed class UpdateDetailsVmValidator : SantaUserValidator<UpdateDetailsVm>
{
    public UpdateDetailsVmValidator()
    {
        RuleFor(x => x.SelectGender).NotNull();
    }
}
