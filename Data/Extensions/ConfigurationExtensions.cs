using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data.Extensions;

public static class ConfigurationExtensions
{
    public static string GetConnectionString(this IConfigurationRoot configuration, 
        SqlConnectionStringBuilder connectionStringBuilder)
    {
        string? overrideDataSource = configuration["DatabaseSettings:SecretSantaDatabaseServer"];

        if (!string.IsNullOrWhiteSpace(overrideDataSource))
        {
            connectionStringBuilder.DataSource = overrideDataSource;
        }

        connectionStringBuilder.UserID = configuration["DatabaseSettings:SecretSantaUserId"];
        connectionStringBuilder.Password = configuration["DatabaseSettings:SecretSantaPassword"];

        string connectionString = connectionStringBuilder.ConnectionString;
        return connectionString;
    }

    public static void ConfigureDatabaseOptions(this DbContextOptionsBuilder optionsBuilder, string connectionString)
    {
        optionsBuilder
            .UseLazyLoadingProxies()
            .UseSqlServer(connectionString);
    }
}
