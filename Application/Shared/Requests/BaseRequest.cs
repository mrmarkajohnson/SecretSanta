using AutoMapper;
using Data;
using Global.Abstractions.Areas.Account;
using Global.Extensions.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Application.Shared.Requests;

public abstract class BaseRequest<TResult>
{
    private UserManager<IdentityUser> _userManager;
    private SignInManager<IdentityUser> _signInManager;

    protected IServiceProvider Services { get; private set; }
    protected ApplicationDbContext DbContext { get; private set; }
    protected IMapper Mapper { get; set; }

    protected ClaimsPrincipal ClaimsUser { get; private set; }
    public bool ClaimsUserNotRequired { get; set; }

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
        DbContext = new ApplicationDbContext();
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

        DbContext = Services.GetService<ApplicationDbContext>() ?? new ApplicationDbContext();

        if (Mapper == null)
        {
            throw new ArgumentException("Mapper cannot be null");
        }

        ClaimsUser = claimsUser;

        if (!ClaimsUserNotRequired)
        {
            string? currentUserId = GetCurrentUserId();
            DbContext.CurrentUserId = currentUserId;
        }
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

    protected Santa_User GetCurrentSantaUser(params Expression<Func<Santa_User, object?>>[] includes)
    {
        Santa_User? dbSantaUser = null;

        EnsureSignedIn();

        string? globalUserId = GetCurrentUserId();
        if (globalUserId != null)
        {
            dbSantaUser = GetSantalUser(globalUserId, includes);
        }

        if (dbSantaUser == null)
        {
            throw new AccessDeniedException();
        }

        return dbSantaUser;
    }

    protected Santa_User? GetSantalUser(string globalUserId, params Expression<Func<Santa_User, object?>>[] includes)
    {
        if (includes != null && includes.Any())
        {
            var query = DbContext.Santa_Users;
            return includes
                .Aggregate(query.AsQueryable(), (current, include) => current.Include(include))
                .FirstOrDefault(x => x.GlobalUserId == globalUserId);
        }
        else
        {
            return DbContext.Santa_Users.FirstOrDefault(x => x.GlobalUserId == globalUserId);
        }
    }

    protected Global_User GetCurrentGlobalUser(params Expression<Func<Global_User, object?>>[] includes)
    {
        Global_User? dbCurrentUser = null;

        EnsureSignedIn();

        string? globalUserId = GetCurrentUserId();
        if (globalUserId != null)
        {
            dbCurrentUser = GetGlobalUser(globalUserId, includes);
        }

        if (dbCurrentUser == null)
        {
            throw new AccessDeniedException();
        }

        return dbCurrentUser;
    }

    protected Global_User? GetGlobalUser(IIdentityUser identityUser, params Expression<Func<Global_User, object?>>[] includes)
    {
        return GetGlobalUser(identityUser.GlobalUserId, includes);
    }

    protected Global_User? GetGlobalUser(string globalUserId, params Expression<Func<Global_User, object?>>[] includes)
    {
        if (includes != null && includes.Any())
        {
            var query = DbContext.Global_Users;
            return includes
                .Aggregate(query.AsQueryable(), (current, include) => current.Include(include))
                .FirstOrDefault(x => x.Id == globalUserId);
        }
        else
        {
            return DbContext.Global_Users.FirstOrDefault(x => x.Id == globalUserId);
        }
    }

    protected async Task<bool> AccessFailed(UserManager<IdentityUser> userManager, IIdentityUser identityUser)
    {
        if (identityUser != null)
        {
            var dbGlobalUser = await userManager.FindByIdAsync(identityUser.GlobalUserId); // always get the user again, to avoid double tracking errors
            if (dbGlobalUser != null)
            {
                await userManager.AccessFailedAsync(dbGlobalUser);
                return dbGlobalUser.LockoutEnd != null && dbGlobalUser.LockoutEnd > DateTime.Now;
            }
        }

        return false;
    }

    protected static string DisplayLink(string url, string display, bool addQuotes)
    {
        return LinkHelper.DisplayLink(url, display, addQuotes);
    }
}
