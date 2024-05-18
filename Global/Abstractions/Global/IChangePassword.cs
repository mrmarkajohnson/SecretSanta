namespace Global.Abstractions.Global;

public interface IChangePassword
{
    string EmailOrUserName { get; }
    string Password { get; }
    string ConfirmPassword { get; }
}
