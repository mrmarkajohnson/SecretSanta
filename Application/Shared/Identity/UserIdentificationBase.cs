using System.ComponentModel.DataAnnotations;

namespace Application.Shared.Identity;

public class UserIdentificationBase : HasEmailBase, IHashableUserBase
{
    [Display(Name = UserDisplayNames.UserName)]
    public virtual string? UserName { get; set; }
}