using System.ComponentModel.DataAnnotations;

namespace Application.Shared.Identity;

public abstract class BaseUser : IHashableUserId
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Display(Name = "Username")]    
    public virtual string? UserName { get; set; }

    [Display(Name = "E-mail Address")]
    public virtual string? Email { get; set; }

    public bool IdentificationHashed { get; set; }
}
