using System.ComponentModel.DataAnnotations;
using Global.Abstractions.Global.Account;
using Global.Validation;

namespace Application.Shared.Identity;

public class CoreIdentityUser : BaseUser, IIdentityUser
{
    [Display(Name = "Username")]
    [StringLength(IdentityVal.UserNames.MaxLength, ErrorMessage = "{0} must be {2} to {1} characters long, if entered.", MinimumLength = IdentityVal.UserNames.MinLength)]
    public override string? UserName { get; set; }

    [EmailAddress(ErrorMessage = "E-mail Address is not valid e-mail.")]
    [Display(Name = "E-mail Address")]
    public override string? Email { get; set; }

    public string Greeting { get; set; } = string.Empty;
}