using Global.Abstractions.Identity;

namespace Application.BaseModels.Identity;

public class GlobalUser : IdentityUser, IGlobalUser
{
    public required string Forename { get; set; }
    public string? MiddleNames { get; set; }
    public required string Surname { get; set; }
}
