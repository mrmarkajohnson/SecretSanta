using Global.Abstractions.Areas.Account;
using System.ComponentModel.DataAnnotations;
using ViewLayer.Abstractions;

namespace ViewLayer.Models.Account;

public sealed class LoginVm : BaseFormVm, ILogin, IFormVm
{
    //public IList<AuthenticationScheme> ExternalLogins { get; set; }

    [Display(Name = $"{UserDisplayNames.Email} or {UserDisplayNames.UserName}")]
    public string EmailOrUserName { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }

    public bool TimedOut { get; set; }
    public bool LockedOut { get; set; }

    public override string SubmitButtonText { get; set; } = "Log in";
    public override string SubmitButtonIcon { get; set; } = "fa-key";
}
