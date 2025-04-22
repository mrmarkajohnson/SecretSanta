using System.ComponentModel.DataAnnotations;

namespace Application.Shared.Identity;

public class UserIdentificationBase : HasEmailBase, IHashableUserBase
{
    [Display(Name = "Username")]
    public virtual string? UserName { get; set; }
}