using Global.Abstractions.Global;
using System.ComponentModel.DataAnnotations;
using ViewLayer.Models.Shared;

namespace ViewLayer.Models.Account;

public class LoginVm : BaseFormVm, IForm
{
    //public IList<AuthenticationScheme> ExternalLogins { get; set; }

    [Display(Name = "E-mail or Username")]
    public required string EmailOrUserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }

    public override string SubmitButtonText { get; set; } = "Log in";
    public override string SubmitButtonIcon { get; set; } = "fa-key";
}
