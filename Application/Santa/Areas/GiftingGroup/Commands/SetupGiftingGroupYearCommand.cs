using Application.Santa.Areas.GiftingGroup.BaseModels;
using Application.Santa.Areas.GiftingGroup.Queries.Internal;
using Global.Abstractions.Santa.Areas.GiftingGroup;
using Global.Extensions.Exceptions;
using static Global.Settings.GiftingGroupSettings;

namespace Application.Santa.Areas.GiftingGroup.Commands;

public class SetupGiftingGroupYearCommand<TItem> : BaseCommand<TItem> where TItem : IGiftingGroupYear
{
    public SetupGiftingGroupYearCommand(TItem item) : base(item)
    {
    }

    public struct Combination
    {
        public int GiverId;
        public int RecipientId;
    }

    public class CombinationNumber
    {
        public int GiverId { get; init; }
        public int PossibleCombinationCount { get; set; }
    }

    protected async override Task<ICommandResult<TItem>> HandlePostValidation()
    {
        if (Item.GiftingGroupId == 0)
        {
            throw new NotFoundException("Gifting Group");
        }

        if (Item.Year == 0)
        {
            Item.Year = DateTime.Today.Year;
        }
        else if (Item.Year != DateTime.Today.Year)
        {
            throw new ArgumentException($"You cannot set up year {Item.Year} as it is not the current year."); // TODO: Allow years to continue into January, just in case
        }

        Santa_GiftingGroupUser dbGiftingGroupLink = await Send(new GetGiftingGroupUserLinkQuery(Item.GiftingGroupId, true));
        Santa_GiftingGroup dbGroup = dbGiftingGroupLink.GiftingGroup;
        Santa_GiftingGroupYear? dbGiftingGroupYear = dbGroup.Years.FirstOrDefault(x => x.Year == Item.Year);

        if (dbGiftingGroupYear == null)
        {
            dbGiftingGroupYear = new Santa_GiftingGroupYear
            {
                GiftingGroup = dbGroup,
                Year = Item.Year
            };

            dbGroup.Years.Add(dbGiftingGroupYear);
            DbContext.ChangeTracker.DetectChanges();
        }

        foreach (IYearGroupUserBase member in Item.GroupMembers)
        {
            Santa_YearGroupUser? dbYearUser = dbGiftingGroupYear.Users.FirstOrDefault(x => x.SantaUserId == member.SantaUserId);

            if (dbYearUser == null)
            {
                var dbSantaUser = DbContext.Santa_Users.FirstOrDefault(x => x.Id == member.SantaUserId);

                if (dbSantaUser == null)
                {
                    AddGeneralValidationError($"User {member.UserDisplayName} could not be found.");
                }
                else
                {
                    dbYearUser = new Santa_YearGroupUser
                    {
                        YearId = dbGiftingGroupYear.Id,
                        Year = dbGiftingGroupYear,
                        SantaUserId = member.SantaUserId,
                        SantaUser = dbSantaUser,
                        Included = member.Included
                    };
                }
            }
        }

        if (!Validation.IsValid)
            return await Result();

        if (Item.CalculationOption == YearCalculationOption.Calculate)
        {
            DateTime firstDayOfNextYear = new DateTime(Item.Year + 1, 1, 1);

            var validGroupMembers = dbGroup.UserLinks
                .Where(x => x.DateDeleted == null && (x.DateArchived == null || x.DateArchived < firstDayOfNextYear));

            var missingGroupMembers = validGroupMembers
                .Where(x => dbGroup.UserLinks.Any(y => y.SantaUserId == x.SantaUserId) == false
                    || Item.GroupMembers.Any(y => y.SantaUserId == x.SantaUserId == false));

            if (missingGroupMembers.Any())
            {
                foreach (var groupMember in missingGroupMembers)
                {
                    Item.GroupMembers.Add(Mapper.Map(groupMember, new YearGroupUserBase()));
                }

                AddGeneralValidationError("New members have been added to the group. Please try again.");
            }
            else
            {
                try
                {
                    CalculateGiversAndReceivers(dbGroup, dbGiftingGroupYear, validGroupMembers);
                }
                catch (Exception exp)
                {
                    AddGeneralValidationError("Error calculating results for year " + Item.Year.ToString() + ": " + exp.Message);
                    return await Result();
                }
            }

            // TODO: Create messages to users telling them their recipient
        }
        else if (Item.CalculationOption == YearCalculationOption.Cancel)
        {
            foreach (var user in dbGiftingGroupYear.Users)
            {
                user.GivingToUserId = null;
                user.GivingToUser = null;
            }

            // TODO: Create messages to users telling them the setup has been cancelled
        }

        if (!Validation.IsValid)
            return await Result();

        dbGiftingGroupYear.Limit = Item.Limit;

        return await SaveAndReturnSuccess();
    }

    private void CalculateGiversAndReceivers(Santa_GiftingGroup dbGroup, Santa_GiftingGroupYear dbGiftingGroupYear, IEnumerable<Santa_GiftingGroupUser> validGroupMembers)
    {
        List<Santa_YearGroupUser> participatingMembers = dbGiftingGroupYear.Users
            .Where(x => x.Included)
            .Where(x => validGroupMembers.Any(y => y.SantaUserId == x.SantaUserId))
            .ToList();

        var possibleCombinations = new List<Combination>();

        var previousYearIDs = dbGroup.Years
            .Where(x => x.Year == Item.Year - 1)
            .SelectMany(y => y.Users)
            .Where(z => z.Included)
            .ToList();

        foreach (Santa_YearGroupUser member in participatingMembers)
        {
            AddMembersToCombinations(participatingMembers, possibleCombinations, previousYearIDs, member);
        }

        var actualCombinations = new List<Combination>(); // TODO: Instead, add a 'selected' bool to possibleCombinations
        bool combinationWorked = false;
        int attempts = 0;        

        while (!combinationWorked && attempts < 10) // TODO: Record the combinations used and try a different combination
        {
            attempts++;
            combinationWorked = TryCalculatingCombination(participatingMembers, possibleCombinations, actualCombinations);
        }

        if (!combinationWorked)
        {
            AddGeneralValidationError("Error calculating results for year " + Item.Year.ToString()
                + ": " + "no possible combinations available");
        }

        foreach (Combination combi in actualCombinations)
        {
            participatingMembers.First(x => x.SantaUserId == combi.GiverId).GivingToUserId = combi.RecipientId;
        }
    }

    private bool TryCalculatingCombination(List<Santa_YearGroupUser> participatingMembers, List<Combination> possibleCombinations, List<Combination> actualCombinations)
    {
        foreach (Santa_YearGroupUser member in participatingMembers)
        {
            bool combinationFound = FindWorkingCombination(member, possibleCombinations, actualCombinations);
            if (combinationFound == false)
                return false;
        }

        return true;
    }

    private void AddMembersToCombinations(List<Santa_YearGroupUser> activeMembers, List<Combination> possibleCombinations, List<Santa_YearGroupUser> previousYearIDs, Santa_YearGroupUser member)
    {
        var partnerIDs = DbContext.Santa_PartnerLinks
            .Where(p => p.ConfirmedByPartner2 && p.RelationshipEnded == null && p.SuggestedById == member.SantaUserId)
            .Select(p => p.ConfirmedById).ToList();

        var partneredIDs = DbContext.Santa_PartnerLinks
            .Where(p => p.ConfirmedByPartner2 && p.RelationshipEnded == null && p.ConfirmedById == member.SantaUserId)
            .Select(p => p.SuggestedById).ToList();

        var partners = partnerIDs.Union(partneredIDs);

        var possibleRecipients = activeMembers
            .Where(am => am.SantaUserId != member.SantaUserId
                && !partnerIDs.Contains(am.SantaUserId)
                && !previousYearIDs.Any(u => am.SantaUserId == u.GivingToUserId));

        foreach (var pr in possibleRecipients)
        {
            var combination = new Combination { GiverId = member.SantaUserId, RecipientId = pr.SantaUserId };
            possibleCombinations.Add(combination);
        }
    }

    private bool FindWorkingCombination(Santa_YearGroupUser member, List<Combination> possibleCombinations, 
        List<Combination> actualCombinations)
    {
        int giverID = member.SantaUserId;
        int possibilities = possibleCombinations.Where(pc => pc.GiverId == giverID).Count(); ;

        if (possibilities <= 0)
        {
            return false;
        }

        List<int> recipintIDs = possibleCombinations.Where(pc => pc.GiverId == giverID).Select(pc => pc.RecipientId).ToList();
        Random random = new Random(); // TODO: If it doesn't work, record the combinations used and try a different combination
        int recipientPosition = random.Next(0, possibilities - 1);
        int recipientId = recipintIDs.ElementAt(recipientPosition);

        var combination = new Combination { GiverId = giverID, RecipientId = recipientId };
        actualCombinations.Add(combination);

        UpdateCombinationLists(possibleCombinations, giverID, recipientId);

        return true;
    }

    private static void UpdateCombinationLists(List<Combination> possibleCombinations, int giverID, int recipientID)
    {
        // ensure the giver and receiver can no longer be chosen
        possibleCombinations.RemoveAll(pc => pc.GiverId == giverID);
        possibleCombinations.RemoveAll(pc => pc.RecipientId == recipientID);
    }
}
