using Global.Abstractions.Identity;
using System.ComponentModel.DataAnnotations;

namespace Application.BaseModels.Identity;

public class IdentityUser : IIdentityUser
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? UserName { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    //public string? PhoneNumber { get; set; }
}
