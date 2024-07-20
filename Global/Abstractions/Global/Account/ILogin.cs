namespace Global.Abstractions.Global.Account;

public interface ILogin : ICheckLockout
{
    string EmailOrUserName { get; set; }
    string Password { get; set; }
    bool RememberMe { get; set; }
}
