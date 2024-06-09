namespace Global.Abstractions.Global;

public interface IChangePassword : ISetPassword
{
    string EmailOrUserName { get; }
}
