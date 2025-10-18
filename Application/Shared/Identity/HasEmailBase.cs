using System.ComponentModel.DataAnnotations;

namespace Application.Shared.Identity;

public abstract class HasEmailBase : IHaveAnEmail
{
    [EmailAddress(ErrorMessage = $"{UserDisplayNames.Email} is not a valid {UserDisplayNames.EmailLower}.")]
    [Display(Name = UserDisplayNames.Email)]
    public virtual string? Email { get; set; }

    public bool ShowEmail { get; set; }
}
