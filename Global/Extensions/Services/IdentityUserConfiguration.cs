using Global.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class IdentityUserConfiguration
{
    public static void AddIdentity<TContext>(this IServiceCollection services) where TContext : IdentityDbContext
    {
        services.AddDefaultIdentity<IdentityUser>(options =>
        {
            options.SignIn.RequireConfirmedEmail = IdentityVal.SignIn.RequireConfirmedEmail;
            options.SignIn.RequireConfirmedPhoneNumber = IdentityVal.SignIn.RequireConfirmedPhoneNumber;
            options.SignIn.RequireConfirmedAccount = IdentityVal.SignIn.RequireConfirmedAccount;
        }).AddEntityFrameworkStores<TContext>();

    }
}
