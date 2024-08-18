using Application.Santa.Areas.GiftingGroup.BaseModels;
using Data.Entities.Shared;
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
        Global_User? globalUserDb = GetCurrentGlobalUser(_user, _signInManager, _userManager, 
            g => g.SantaUser, g => g.SantaUser.GiftingGroupLinks);

        if (globalUserDb != null)
        {
            var santaUser = globalUserDb.SantaUser;

            if (santaUser != null)
            {
                var giftingGroupLink = santaUser.GiftingGroupLinks.FirstOrDefault(x => x.GiftingGroupId == _groupId);

                if (giftingGroupLink != null)
                {
                    if (giftingGroupLink.GroupAdmin)
                    {
                        return Mapper.Map<IGiftingGroup>(giftingGroupLink.GiftingGroup);
                    }
                    else
                    {
                        throw new AccessDeniedException();
                    }
                }
            }
        }

        return new CoreGiftingGroup();
    }
}
