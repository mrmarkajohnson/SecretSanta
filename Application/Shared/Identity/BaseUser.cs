namespace Application.Shared.Identity;

public abstract class BaseUser : UserIdentificationBase, IHashableUser, IHasHashedUserId
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
