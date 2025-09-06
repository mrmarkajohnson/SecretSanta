using Global.Settings;
using Web.Configuration;

namespace Web.Extensions;

public static class ConfigurationExtensions
{
    public static void ConfigureEmail(this WebApplicationBuilder builder)
    {
        var emailSection = builder.Configuration.GetSection("Email");
        builder.Services.Configure<MailSettings>(emailSection);
        var mailSettings = emailSection.Get<MailSettings>();

        if (mailSettings == null)
            return;

        AddConfigVariables(builder, mailSettings);

        builder.Services.AddSingleton<IMailSettings>(mailSettings);
    }

    private static void AddConfigVariables(WebApplicationBuilder builder, MailSettings mailSettings)
    {
        string? fromAddress = builder.Configuration[ConfigurationSettings.EmailFromAddress];

        if (fromAddress.IsNotEmpty())
        {
            mailSettings.FromAddress = fromAddress;
        }

        string? smtpHost = builder.Configuration[ConfigurationSettings.EmailHost];

        if (smtpHost.IsNotEmpty())
        {
            mailSettings.Host = smtpHost;
        }

        string? userName = builder.Configuration[ConfigurationSettings.EmailUserName];

        if (userName.IsNotEmpty())
        {
            mailSettings.UserName = userName;
        }

        string? password = builder.Configuration[ConfigurationSettings.EmailPassword];

        if (password.IsNotEmpty())
        {
            mailSettings.Password = password;
        }

        string? testAddress = builder.Configuration[ConfigurationSettings.EmailTestAddress];

        if (testAddress.IsNotEmpty())
        {
            mailSettings.TestAddress = testAddress;
        }
    }

    //private static void Configure<TClass, TInterface>(this WebApplicationBuilder builder, string sectionName)
    //    where TClass : class, TInterface, new() where TInterface : class
    //{
    //    var section = builder.Configuration.GetSection(sectionName);
    //    builder.Services.Configure<TClass>(section);
    //    var settings = section.Get<TClass>();
    //    builder.Services.AddSingleton<TInterface>(settings);
    //}
}
