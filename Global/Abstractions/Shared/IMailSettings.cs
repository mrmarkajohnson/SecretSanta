namespace Global.Abstractions.Shared;

public interface IMailSettings
{
    string FromName { get; }
    string FromAddress { get; }
    string Host { get; }
    int Port { get; }
    bool UseSSL { get; }
    string UserName { get; }
    string Password { get; }
}
