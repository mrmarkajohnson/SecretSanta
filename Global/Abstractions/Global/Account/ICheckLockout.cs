namespace Global.Abstractions.Global.Account;

public interface ICheckLockout
{
    bool LockedOut { get; set; }
}
