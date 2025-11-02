namespace Application.Shared.Identity;

public abstract class BaseUser : UserIdentificationBase, IHashableUser, IHaveAHashedUserId
{
    private string? _globalUserId;
    private string? _hashedUserId;

    public string GlobalUserId
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_globalUserId) && _hashedUserId.IsNotEmpty())
            {
                _globalUserId = this.GetStringUserId();
            }

            return _globalUserId ?? Guid.NewGuid().ToString();
        }
        set
        {
            if (value.IsNotEmpty() && value != Guid.Empty.ToString())
            {
                _globalUserId = value;
            }
        }
    }

    public bool IdentificationHashed { get; set; }

    public string HashedUserId
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_hashedUserId) && _globalUserId.IsNotEmpty())
            {
                _hashedUserId = this.GetHashedUserId();
            }

            return _hashedUserId ?? "";
        }
        set
        {
            if (value.IsNotEmpty())
            {
                _hashedUserId = value;
            }
        }
    }
}
