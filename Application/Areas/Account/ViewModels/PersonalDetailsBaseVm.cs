using Application.Areas.Account.BaseModels;
using Global.Abstractions.ViewModels;
using System.ComponentModel.DataAnnotations;
using static Global.Settings.GlobalSettings;

namespace Application.Areas.Account.ViewModels;

public abstract class PersonalDetailsBaseVm : SantaUser, IFormVm
{
    private Gender? _selectGender { get; set; }

    [Display(Name = "Gender")]
    public Gender? SelectGender 
    {
        get => _selectGender;
        set
        {
            _selectGender = value;
            Gender = value ?? Gender.Other;
        }
    }

    public string? ReturnUrl { get; set; }
    public string? SuccessMessage { get; set; }
    public virtual string SubmitButtonText { get; set; } = string.Empty;
    public virtual string SubmitButtonIcon { get; set; } = string.Empty;
}
