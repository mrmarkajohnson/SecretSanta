namespace Global.Settings;

public class ConfigurationSettings
{
    public const string DatabaseServer = "DatabaseSettings:SecretSantaDatabaseServer";
    public const string DatabaseUser = "DatabaseSettings:SecretSantaUserId";
    public const string DatabasePassword = "DatabaseSettings:SecretSantaPassword";
    public const string SymmetricKeyEnd = "EncryptionSettings:SecretSantaSymmetricKeyEnd";
    public const string EmailFromName = "MailSettings:FromName";
    public const string EmailFromAddress = "MailSettings:FromAddress";
    public const string EmailHost = "MailSettings:SmtpHost";
    public const string EmailUserName = "MailSettings:UserName";
    public const string EmailPassword = "MailSettings:Password";
}
