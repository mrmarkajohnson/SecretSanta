using AutoMapper;
using Data.Entities.Shared;
using Global.Abstractions.Global.Account;
using Global.Extensions.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SecretSanta.Data;
using System.Linq.Expressions;
using System.Security.Claims;

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

    protected void EnsureSignedIn(ClaimsPrincipal user, SignInManager<IdentityUser> signInManager)
    {
        if (!signInManager.IsSignedIn(user))
        {
            throw new NotSignedInException();
        }
    }

    protected Global_User? GetCurrentGlobalUser(ClaimsPrincipal user, 
        SignInManager<IdentityUser> signInManager, 
        UserManager<IdentityUser> userManager, 
        params Expression<Func<Global_User, object>>[] includes)
    {
        Global_User? globalUserDb = null;

        EnsureSignedIn(user, signInManager);

        string? userId = userManager.GetUserId(user);
        if (userId != null)
        {
            globalUserDb = GetGlobalUser(userId);
        }

        return globalUserDb;
    }

    protected Global_User? GetGlobalUser(IIdentityUser user, params Expression<Func<Global_User, object>>[] includes)
    {
        return GetGlobalUser(user.Id, includes);
    }

    protected Global_User? GetGlobalUser(string userId, params Expression<Func<Global_User, object>>[] includes)
    {
        if (includes != null && includes.Any())
        {
            var query = ModelContext.Global_Users;
            return includes
                .Aggregate(query.AsQueryable(), (current, include) => current.Include(include))
                .FirstOrDefault(x => x.Id == userId);
        }
        else
        {
            return ModelContext.Global_Users.FirstOrDefault(x => x.Id == userId);
        }
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
