using Global.Abstractions.Areas.Account;
using System.ComponentModel.DataAnnotations;

namespace Application.Shared.Identity;

public class CoreIdentityUser : BaseUser, IIdentityUser
{
    [Display(Name = "Username")]
    [StringLength(IdentityVal.UserNames.MaxLength, ErrorMessage = "{0} must be {2} to {1} characters long, if entered.", MinimumLength = IdentityVal.UserNames.MinLength)]
    public override string? UserName { get; set; }

    public string Greeting { get; set; } = string.Empty;
}