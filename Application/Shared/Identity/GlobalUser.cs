using Global.Abstractions.Global;

namespace Application.Shared.Identity;

public class GlobalUser : IdentityUser, IGlobalUser
{
    public required string Forename { get; set; }
    public string? MiddleNames { get; set; }
    public required string Surname { get; set; }
}
