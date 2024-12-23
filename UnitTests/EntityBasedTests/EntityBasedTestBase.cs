using Global.Extensions.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;
using SecretSanta.Data;
using System.Security.Claims;
using UnitTests.Context;

namespace UnitTests.EntityBasedTests;

public abstract class EntityBasedTestBase
{
    protected ClaimsPrincipal CurrentUser => new ClaimsPrincipal();

    private protected static ServiceProvider GetServiceProvider(TestDbContext context)
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddSingleton<TestDbContext>(context);

        builder.Services.AddIdentity<TestDbContext>();
        builder.Services.ConfigureAutoMapperProfiles();

        HostingEnvironment env = new HostingEnvironment();
        env.ContentRootPath = Directory.GetCurrentDirectory();
        env.EnvironmentName = "Development";

        ServiceProvider ServiceProvider = builder.Services.BuildServiceProvider();
        return ServiceProvider;
    }
}
