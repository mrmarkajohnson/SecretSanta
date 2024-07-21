using AutoMapper;
using Data.Entities.Shared;
using Global.Abstractions.Global.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SecretSanta.Data;

namespace Application.Santa.Global;

public abstract class BaseRequest<TResult>
{
    protected IServiceProvider Services { get; set; }
    protected ApplicationDbContext ModelContext { get; set; }
    protected IMapper Mapper { get; set; }

    #pragma warning disable CS8618
    protected BaseRequest()
    {
        ModelContext = new ApplicationDbContext();
    }
    #pragma warning restore CS8618

    protected void Initialise(IServiceProvider services)
    {
        if (services == null)
        {
            throw new ArgumentException("Services cannot be null");
        }

        Services = services;
        Mapper = services.GetRequiredService<IMapper>();

        if (Mapper == null)
        {
            throw new ArgumentException("Mapper cannot be null");
        }
    }

    public abstract Task<TResult> Handle(IServiceProvider Services);

    protected async Task<TItem> Send<TItem>(BaseQuery<TItem> query)
    {
        if (Services == null)
        {
            throw new ArgumentException("Services cannot be null");
        }

        return await query.Handle(Services);
    }

    protected async Task<bool> Send<TItem>(BaseAction<TItem> action)
    {
        if (Services == null)
        {
            throw new ArgumentException("Services cannot be null");
        }

        return await action.Handle(Services);
    }

    protected Global_User? GetGlobalUser(IIdentityUser user)
    {
        return GetGlobalUser(user.Id);
    }

    protected Global_User? GetGlobalUser(string userId)
    {
        return ModelContext.Global_Users.FirstOrDefault(x => x.Id == userId);
    }

    protected async Task<bool> AccessFailed(UserManager<IdentityUser> userManager, IIdentityUser user)
    {
        if (user != null)
        {
            var dbUser = await userManager.FindByIdAsync(user.Id); // always get the user again, to avoid double tracking errors
            if (dbUser != null)
            {
                await userManager.AccessFailedAsync(dbUser);
                return dbUser.LockoutEnd != null && dbUser.LockoutEnd > DateTime.Now;
            }
        }

        return false;        
    }
}
