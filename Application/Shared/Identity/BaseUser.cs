using System.ComponentModel.DataAnnotations;

namespace Application.Shared.Identity;

public abstract class BaseUser : IHashableUser, IHasHashedUserId
{
    private string? _globalUserId;
    private string? _hashedUserId;

    public string GlobalUserId
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_globalUserId))
            {
                _globalUserId = this.GetStringUserId();
            }

            return _globalUserId ?? Guid.NewGuid().ToString();
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value) && value != Guid.Empty.ToString())
            {
                _globalUserId = value;
            }
        }
    }

    [Display(Name = "Username")]    
    public virtual string? UserName { get; set; }

    [Display(Name = "E-mail Address")]
    public virtual string? Email { get; set; }

    public bool IdentificationHashed { get; set; }

    public string HashedUserId
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_hashedUserId))
            {
                _hashedUserId = this.GetHashedUserId();
            }

            return _hashedUserId;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                _hashedUserId = value;
            }
        }
    }
}
