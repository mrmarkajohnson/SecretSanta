using Global.Abstractions.Global;
using System.ComponentModel.DataAnnotations;

namespace Application.Shared.Identity;

public class IdentityUser : IIdentityUser
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Display(Name = "Username")]
    public string? UserName { get; set; }

    [EmailAddress]
    [Display(Name = "E-mail")]
    public string? Email { get; set; }

    //public string? PhoneNumber { get; set; }
}
