using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Global.Extensions.Services;

public static class AutoMapperConfiguration
{
    public static void ConfigureAutoMapperProfiles(this IServiceCollection services)
    {
        string[] mapperAssemblyNames = ["Application", "ViewLayer"];
        Assembly[] mapperAssemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => mapperAssemblyNames.Contains(x.GetName().Name))
            .ToArray();

        var profiles = mapperAssemblies.SelectMany(x => x.GetTypes()
            .Where(x => typeof(Profile).IsAssignableFrom(x)))
            .ToArray();

        services.AddAutoMapper(profiles);
        services.Configure<IMapper>(cfg => cfg.ConfigurationProvider
            .AssertConfigurationIsValid()); // test mappings to make sure they are OK
    }
}
