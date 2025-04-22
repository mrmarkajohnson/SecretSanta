using System.ComponentModel.DataAnnotations;

namespace Application.Shared.Identity;

public abstract class HasEmailBase : IHasEmail
{
    [EmailAddress(ErrorMessage = "E-mail Address is not valid e-mail.")]
    [Display(Name = "E-mail Address")]
    public virtual string? Email { get; set; }

    public bool ShowEmail { get; set; }
}
