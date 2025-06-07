namespace Application.Shared.Identity;

public abstract class BaseUser : UserIdentificationBase, IHashableUser, IHasHashedUserId
{
    private string? _globalUserId;
    private string? _hashedUserId;

    public string GlobalUserId
    {
        get
        {
            if (string.IsNullOrWhiteSpace(_globalUserId) && _hashedUserId.NotEmpty())
            {
                _globalUserId = this.GetStringUserId();
            }

            return _globalUserId ?? Guid.NewGuid().ToString();
        }
        set
        {
            if (value.NotEmpty() && value != Guid.Empty.ToString())
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
            if (string.IsNullOrWhiteSpace(_hashedUserId) && _globalUserId.NotEmpty())
            {
                _hashedUserId = this.GetHashedUserId();
            }

            return _hashedUserId ?? "";
        }
        set
        {
            if (value.NotEmpty())
            {
                _hashedUserId = value;
            }
        }
    }
}
