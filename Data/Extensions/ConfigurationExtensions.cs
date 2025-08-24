using Global.Settings;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data.Extensions;

public static class ConfigurationExtensions
{
    public static string GetConnectionString(this IConfiguration configuration, 
        SqlConnectionStringBuilder connectionStringBuilder)
    {
        string? overrideDataSource = configuration[ConfigurationSettings.DatabaseServer];

        if (!string.IsNullOrWhiteSpace(overrideDataSource))
        {
            connectionStringBuilder.DataSource = overrideDataSource;
        }

        connectionStringBuilder.UserID = configuration[ConfigurationSettings.DatabaseUser];
        connectionStringBuilder.Password = configuration[ConfigurationSettings.DatabasePassword];

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
