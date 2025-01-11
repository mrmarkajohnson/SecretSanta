namespace Global.Abstractions.Areas.Account;

public interface ICheckLockout
{
    bool LockedOut { get; set; }
}
