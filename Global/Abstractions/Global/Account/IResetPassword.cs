namespace Global.Abstractions.Global.Account;

public interface IResetPassword : ISetPassword
{
    string EmailOrUserName { get; }
}
