using Data.Entities.Data.Santa;
using Global.Abstractions.Identity;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities.Data.Global;

[Table("Global_User")]
public class Global_User : IdentityUser, IGlobalUser
{
    [Required, Length(2, 250)]
    public required string Forename { get; set; }

    public string? MiddleNames { get; set; }

    [Required, Length(2, 250)]
    public required string Surname { get; set; }

    public virtual Santa_User? SantaUser { get; set; }
}
