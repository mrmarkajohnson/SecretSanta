namespace Global.Settings;

public class ConfigurationSettings
{
    /// <summary>
    /// Server address of the database, relative to the application
    /// </summary>
    public const string DatabaseServer = "DatabaseSettings:SecretSantaDatabaseServer";

    /// <summary>
    /// User to connect to the database, which must have read-write access
    /// </summary>
    public const string DatabaseUser = "DatabaseSettings:SecretSantaUserId";

    /// <summary>
    /// Password to connect to the database - keep this extra secure!
    /// </summary>
    public const string DatabasePassword = "DatabaseSettings:SecretSantaPassword";

    /// <summary>
    /// 'Random' string to use for hashing data that can be unhashed; ideally about 10-12 characters
    /// </summary>
    public const string SymmetricKeyEnd = "EncryptionSettings:SecretSantaSymmetricKeyEnd";

    /// <summary>
    /// The from name to display to the recipient on e-mails; set in appsettings.json as "Santa", but could be overridden
    /// </summary>
    public const string EmailFromName = "MailSettings:FromName";

    /// <summary>
    /// The from address to display to the recipient on e-mails, e.g. noreply@secretsanta.{yourdomain}
    /// </summary>
    public const string EmailFromAddress = "MailSettings:FromAddress";

    /// <summary>
    /// Address of the SMTP host, relative to the application
    /// </summary>
    public const string EmailHost = "MailSettings:SmtpHost";

    /// <summary>
    /// User name to connect to the SMTP host, if it needs authentication
    /// </summary>
    public const string EmailUserName = "MailSettings:UserName";

    /// <summary>
    /// Password to connect to the SMTP host, if it needs authentication - keep this extra secure!
    /// </summary>
    public const string EmailPassword = "MailSettings:Password";

    /// <summary>
    /// For non-production, any e-mails will go to this address, except when using the e-mail test harness
    /// </summary>
    public const string EmailTestAddress = "MailSettings:TestAddress";

    /// <summary>
    /// The base URL of the application, set automatically on startup (see Program.cs in the Web project)
    /// </summary>
    public static string? BaseUrl { get; set; }    
}
