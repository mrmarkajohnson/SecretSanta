using System.ComponentModel.DataAnnotations;
using Global.Validation;

namespace Application.Shared.Identity;

public class CoreIdentityUser : IIdentityUser
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Display(Name = "Username")]
    [StringLength(IdentityVal.UserNames.MaxLength, ErrorMessage = "{0} must be {2} to {1} characters long, if entered.", MinimumLength = IdentityVal.UserNames.MinLength)]
    public string? UserName { get; set; }

    [EmailAddress(ErrorMessage = "E-mail is not a valid e-mail address.")]
    [Display(Name = "E-mail")]
    public string? Email { get; set; }

    //public string? PhoneNumber { get; set; }

    public string Greeting { get; set; } = "";

    public bool IdentificationHashed { get; set; }
}