using Global.Extensions.Exceptions;

namespace Application.Areas.GiftingGroup.Queries.Internal;

internal class GetInvitationEntityQuery : BaseQuery<Santa_Invitation?>
{
    public GetInvitationEntityQuery(string invitationId)
    {
        _invitationId = invitationId;
    }

    public GetInvitationEntityQuery(Guid invitationGuid)
    {
        _invitationGuid = invitationGuid;
    }

    private readonly string? _invitationId;
    private readonly Guid? _invitationGuid;
    private const string _askToResend = "If it should have been for you, please ask the person who sent it to send another, with the correct details.";

    protected override Task<Santa_Invitation?> Handle()
    {
        var dbOpenInvitations = DbContext.Santa_Invitations
            .Where(x => x.DateArchived == null)
            .ToList();

        Santa_Invitation? dbInvitation = null;

        if (_invitationGuid != null)
        {
            dbInvitation = dbOpenInvitations.FirstOrDefault(x => x.InvitationGuid == _invitationGuid);
        }
        else
        {
            dbInvitation = dbOpenInvitations.FirstOrDefault(x => x.GetInvitationId() == _invitationId);
        }

        if (dbInvitation == null)
        {
            throw new NotFoundException("invitation");
        }

        if (!SignInManager.IsSignedIn(ClaimsUser))
            return Result(dbInvitation); // just return it, we can't tell if it's for the current user at this stage

        Santa_User dbCurrentSantaUser = GetCurrentSantaUser();

        bool returnInvitation = false;

        if (dbInvitation.ToSantaUserKey > 0)
        {
            returnInvitation = MatchesOnUser(dbInvitation, dbCurrentSantaUser);
        }
        else if (dbCurrentSantaUser.GlobalUser.Email.IsNotEmpty())
        {
            returnInvitation = MatchesOnEmail(dbInvitation, dbCurrentSantaUser);
        }
        else if (!NameMatches(dbInvitation.ToName, dbCurrentSantaUser))
        {
            throw new AccessDeniedException($"This invitation is for a different {UserDisplayNames.Forename.ToLower()}. {_askToResend}");
        }
        else
        {
            returnInvitation = NameMatchesWithNoEmail(dbInvitation);
        }

        if (returnInvitation)
        {
            return Result(dbInvitation);
        }
        else // shouldn't happen, as each method will throw an exception if not returning true
        {
            throw new NotFoundException("invitation");
        }
    }

    private bool MatchesOnUser(Santa_Invitation dbInvitation, Santa_User dbCurrentSantaUser)
    {
        if (dbCurrentSantaUser != null && dbCurrentSantaUser.SantaUserKey == dbInvitation.ToSantaUserKey)
        {
            return true;
        }
        else
        {
            throw new AccessDeniedException($"This invitation is for a different user. {_askToResend}");
        }
    }

    private bool MatchesOnEmail(Santa_Invitation dbInvitation, Santa_User dbCurrentSantaUser)
    {
        if (!NameMatches(dbInvitation.ToName, dbCurrentSantaUser))
        {
            if (!EmailMatches(dbInvitation, dbCurrentSantaUser))
            {
                throw new AccessDeniedException($"This invitation is for a different {UserDisplayNames.EmailLower}. {_askToResend}");
            }
            else
            {
                throw new AccessDeniedException($"This invitation is for a different {UserDisplayNames.Forename.ToLower()}. {_askToResend}");
            }
        }
        else if (!EmailMatches(dbInvitation, dbCurrentSantaUser))
        {
            throw new AccessDeniedException($"This invitation is for a different {UserDisplayNames.EmailLower}. {_askToResend}");
        }
        else
        {
            dbInvitation.ToSantaUser = dbCurrentSantaUser;
            return true;
        }
    }

    private bool NameMatchesWithNoEmail(Santa_Invitation dbInvitation)
    {
        var matchingUser = DbContext.Users.FirstOrDefault(x => x.Email == dbInvitation.ToEmailAddress);

        if (matchingUser != null)
        {
            throw new AccessDeniedException($"This invitation is for a different user. {_askToResend}");
        }
        else
        {
            return true;
        }
    }

    private static bool EmailMatches(Santa_Invitation dbInvitation, Santa_User dbCurrentSantaUser)
    {
        return dbCurrentSantaUser.GlobalUser.Email?.Tidy() == dbInvitation.ToEmailAddress?.Tidy();
    }

    private bool NameMatches(string? toName, Santa_User dbSantaUser)
    {
        return string.Equals(toName, dbSantaUser.GlobalUser.Forename, StringComparison.InvariantCultureIgnoreCase)
            || string.Equals(toName, dbSantaUser.GlobalUser.PreferredFirstName, StringComparison.InvariantCultureIgnoreCase);
    }
}
