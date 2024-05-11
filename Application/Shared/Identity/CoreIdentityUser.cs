using Global.Abstractions.Global;
using Global.Validation;
using System.ComponentModel.DataAnnotations;

namespace Application.Shared.Identity;

public class CoreIdentityUser : IIdentityUser
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Display(Name = "Username")]
    [StringLength(100, ErrorMessage = "Your {0} must be {2} to {1} characters long, if entered.", MinimumLength = Global.Validation.Identity.UserNames.MinLength)]
    public string? UserName { get; set; }

    [EmailAddress]
    [Display(Name = "E-mail")]
    public string? Email { get; set; }

    //public string? PhoneNumber { get; set; }
}
