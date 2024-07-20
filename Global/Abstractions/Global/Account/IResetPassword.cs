namespace Global.Abstractions.Global.Account;

public interface IResetPassword : ISetPassword, ICheckLockout
{
    string EmailOrUserName { get; }
}
