using System.ComponentModel.DataAnnotations;

namespace Global.Abstractions.Global;

public interface IIdentityUser
{
    string Id { get; set; }

    string? UserName { get; set; }

    [Display(Name = "E-mail Address")]
    string? Email { get; set; }

    //[Display(Name = "Phone Number")]
    //string? PhoneNumber { get; set; }
}
