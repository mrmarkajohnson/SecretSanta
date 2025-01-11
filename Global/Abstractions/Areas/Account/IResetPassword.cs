namespace Global.Abstractions.Areas.Account;

public interface IResetPassword : ISetPassword, ICheckLockout
{
    string EmailOrUserName { get; }
}
