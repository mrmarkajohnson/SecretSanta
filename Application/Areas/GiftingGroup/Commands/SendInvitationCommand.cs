using Global.Abstractions.Areas.GiftingGroup;

namespace Application.Areas.GiftingGroup.Commands;

public class SendInvitationCommand<TItem> : GiftingGroupBaseCommand<TItem> where TItem : ISendGroupInvitation
{
    public SendInvitationCommand(TItem item) : base(item)
    {
    }

    protected override Task<ICommandResult<TItem>> HandlePostValidation()
    {
        Santa_User dbCurrentSantaUser = GetCurrentSantaUser(s => s.GiftingGroupLinks);

        if (Item.ToHashedUserId.IsNotEmpty())
        {
            var dbPossibleToUsers = dbCurrentSantaUser.GiftingGroupLinks
                .Select(x => x.GiftingGroup)
                .Where(x => x.GiftingGroupKey != Item.GiftingGroupKey)
                .SelectMany(y => y.Members)
                .Where(y => y.SantaUserKey != dbCurrentSantaUser.SantaUserKey)
                .Select(x => x.SantaUser)
                .ToList();

            Santa_User? dbToSantaUser = dbPossibleToUsers
                .FirstOrDefault(x => x.GlobalUser.GetHashedUserId() == Item.ToHashedUserId);

            if (dbToSantaUser == null)
            {
                AddGeneralValidationError("No matching user found.");
            }
            else
            {
                SendUserInvitation(dbToSantaUser);
            }
        }
        else
        {
            var dbPossibleToUsers = DbContext.Global_Users
                .Where(x => x.Forename == Item.ToName || x.PreferredFirstName == Item.ToName)
                .Where(y => y.SantaUser != null && y.SantaUser.SantaUserKey != dbCurrentSantaUser.SantaUserKey)
                .ToList();

            Santa_User? dbToSantaUser = dbPossibleToUsers
                .Where(x => x.GetUnhashedDetails().Email == Item.ToEmailAddress)
                .Select(x => x.SantaUser)
                .FirstOrDefault();

            if (dbToSantaUser == null)
            {
                bool emailExists = DbContext.Global_Users
                    .ToList()
                    .Where(x => x.GetUnhashedDetails().Email == Item.ToEmailAddress)
                    .Any();

                if (emailExists)
                {
                    AddGeneralValidationError("A user was found matching the e-mail address entered, but their name did not match.");
                    return Result();
                }
                else
                {
                    SendEmailInvitation();
                }
            }
            else
            {
                SendUserInvitation(dbToSantaUser);
            }
        }

        return Result();
    }

    private void SendUserInvitation(Santa_User dbToSantaUser)
    {
        
        
        
        Success = true;
    }

    private void SendEmailInvitation()
    {




        Success = true;
    }
}
