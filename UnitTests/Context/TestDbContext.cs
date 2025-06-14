using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace UnitTests.Context;

/// <summary>
/// This can't be used to save, or run commands that save etc. as it won't behave as it should
/// It can be used to unit test something that requires an entity, but won't update that entity
/// </summary>
internal class TestDbContext : ApplicationDbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .Build();

            optionsBuilder
                .UseLazyLoadingProxies()
                .UseInMemoryDatabase("TestDatabase");
        }
    }
}
