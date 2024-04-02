using Global.Abstractions.Global;
using System.ComponentModel.DataAnnotations;

namespace Application.Shared.Identity;

public class IdentityUser : IIdentityUser
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? UserName { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    //public string? PhoneNumber { get; set; }
}
