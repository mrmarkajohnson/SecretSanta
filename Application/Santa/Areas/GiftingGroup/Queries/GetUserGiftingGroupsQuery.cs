using Application.Santa.Areas.GiftingGroup.BaseModels;
using Global.Abstractions.Santa.Areas.GiftingGroup;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Santa.Areas.GiftingGroup.Queries;

public class GetUserGiftingGroupsQuery : BaseQuery<IList<IUserGiftingGroup>>
{
    private readonly ClaimsPrincipal _user;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public GetUserGiftingGroupsQuery(ClaimsPrincipal user, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _user = user;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    protected override Task<IList<IUserGiftingGroup>> Handle()
    {
        Global_User? dbGlobalUser = GetCurrentGlobalUser(_user, _signInManager, _userManager,
            g => g.SantaUser, g => g.SantaUser.GiftingGroupLinks);

        ICollection<Santa_GiftingGroupUser>? dbGroupLinks = dbGlobalUser?.SantaUser?.GiftingGroupLinks;
        IList<IUserGiftingGroup> userGroups = new List<IUserGiftingGroup>();

        if (dbGroupLinks?.Any() == true)
        {
            userGroups = dbGroupLinks
                .Where(x => x.DateDeleted == null && x.GiftingGroup != null && x.GiftingGroup.DateDeleted == null)
                .Select(x => new UserGiftingGroup { GroupId = x.GiftingGroupId, GroupName = x.GiftingGroup.Name, GroupAdmin = x.GroupAdmin })
                .ToList<IUserGiftingGroup>();
        }

        return Task.FromResult(userGroups);
    }
}
