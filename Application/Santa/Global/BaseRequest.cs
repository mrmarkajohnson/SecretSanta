using AutoMapper;
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
    private UserManager<IdentityUser> _userManager;
    private SignInManager<IdentityUser> _signInManager;

    protected IServiceProvider Services { get; private set; }
    protected ApplicationDbContext ModelContext { get; private set; }
    protected IMapper Mapper { get; set; }

    protected ClaimsPrincipal ClaimsUser {  get; private set; }

    protected UserManager<IdentityUser> UserManager 
    { 
        get
        {
            if (_userManager == null)
            {
                _userManager = Services.GetRequiredService<UserManager<IdentityUser>>();
            }

            return _userManager;
        }
    }

    protected SignInManager<IdentityUser> SignInManager
    {
        get
        {
            if (_signInManager == null)
            {
                _signInManager = Services.GetRequiredService<SignInManager<IdentityUser>>();
            }

            return _signInManager;
        }
    }

    #pragma warning disable CS8618
    protected BaseRequest()
    {
        ModelContext = new ApplicationDbContext();
    }
    #pragma warning restore CS8618

    protected void Initialise(IServiceProvider services, ClaimsPrincipal claimsUser)
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

        ClaimsUser = claimsUser;
    }

    public abstract Task<TResult> Handle(IServiceProvider Services, ClaimsPrincipal claimsUser);

    protected async Task<TItem> Send<TItem>(BaseQuery<TItem> query)
    {
        if (Services == null)
        {
            throw new ArgumentException("Services cannot be null");
        }

        return await query.Handle(Services, ClaimsUser);
    }

    protected async Task<bool> Send<TItem>(BaseAction<TItem> action)
    {
        if (Services == null)
        {
            throw new ArgumentException("Services cannot be null");
        }

        return await action.Handle(Services, ClaimsUser);
    }

    protected void EnsureSignedIn()
    {
        if (!SignInManager.IsSignedIn(ClaimsUser))
        {
            throw new NotSignedInException();
        }
    }

    protected string? GetCurrentUserId()
    {
        return UserManager.GetUserId(ClaimsUser);
    }

    protected Global_User? GetCurrentGlobalUser(params Expression<Func<Global_User, object>>[] includes)
    {
        Global_User? dbGlobalUser = null;

        EnsureSignedIn();

        string? userId = GetCurrentUserId();
        if (userId != null)
        {
            dbGlobalUser = GetGlobalUser(userId, includes);
        }

        return dbGlobalUser;
    }

    protected Global_User? GetGlobalUser(IIdentityUser identityUser, params Expression<Func<Global_User, object>>[] includes)
    {
        return GetGlobalUser(identityUser.Id, includes);
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

    protected async Task<bool> AccessFailed(UserManager<IdentityUser> userManager, IIdentityUser identityUser)
    {
        if (identityUser != null)
        {
            var dbUser = await userManager.FindByIdAsync(identityUser.Id); // always get the user again, to avoid double tracking errors
            if (dbUser != null)
            {
                await userManager.AccessFailedAsync(dbUser);
                return dbUser.LockoutEnd != null && dbUser.LockoutEnd > DateTime.Now;
            }
        }

        return false;        
    }
}
