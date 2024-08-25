using Application.Santa.Areas.GiftingGroup.BaseModels;
using Global.Abstractions.Santa.Areas.GiftingGroup;
using Global.Extensions.Exceptions;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Santa.Areas.GiftingGroup.Queries;

public class EditGiftingGroupQuery : BaseQuery<IGiftingGroup>
{
    private readonly int _groupId;
    private readonly ClaimsPrincipal _user;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public EditGiftingGroupQuery(int groupId, ClaimsPrincipal user, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _groupId = groupId;
        _user = user;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    protected async override Task<IGiftingGroup> Handle()
    {
        if (_groupId == 0)
        {
            return new CoreGiftingGroup();
        }
        
        Global_User? dbGlobalUser = GetCurrentGlobalUser(_user, _signInManager, _userManager, 
            g => g.SantaUser, g => g.SantaUser.GiftingGroupLinks);

        if (dbGlobalUser != null)
        {
            Santa_User? dbSantaUser = dbGlobalUser.SantaUser;

            if (dbSantaUser != null)
            {
                Santa_GiftingGroupUser? dbGiftingGroupLink = dbSantaUser.GiftingGroupLinks
                    .Where(x => x.DateDeleted == null && x.GiftingGroup.DateDeleted == null)
                    .FirstOrDefault(x => x.GiftingGroupId == _groupId);

                if (dbGiftingGroupLink != null)
                {
                    if (dbGiftingGroupLink.GroupAdmin)
                    {
                        return Mapper.Map<IGiftingGroup>(dbGiftingGroupLink.GiftingGroup);
                    }
                    else
                    {
                        throw new AccessDeniedException();
                    }
                }
            }
        }

        throw new NotFoundException("Gifting Group");
    }
}
