using Application.Santa.Areas.GiftingGroup.BaseModels;

namespace Application.Santa.Areas.GiftingGroup.Queries.Internal;

internal class CalculateGiversAndReceiversQuery : BaseQuery<List<GiverAndReceiverCombination>>
{
    private readonly Santa_GiftingGroupYear _dbGiftingGroupYear;
    private readonly Santa_GiftingGroup _dbGroup;
    private readonly List<Santa_YearGroupUser> _participatingMembers;

    private List<GiverAndReceiverCombination> _possibleCombinations = new();
    private List<GiverAndReceiverCombination> _actualCombinations = new();
    private List<GiverAndReceiverCombination> _failedCombinations = new();

    private int _memberPosition = 0;

    public CalculateGiversAndReceiversQuery(Santa_GiftingGroupYear dbGiftingGroupYear)
    {
        _dbGiftingGroupYear = dbGiftingGroupYear;
        _dbGroup = dbGiftingGroupYear.GiftingGroup;
        _participatingMembers = dbGiftingGroupYear.ParticipatingMembers();
    }

    protected override Task<List<GiverAndReceiverCombination>> Handle()
    {
        SetPossibleCombinations();
        List<GiverAndReceiverCombination> actualCombinations = GetActualCombinations();

        return Task.FromResult(actualCombinations);
    }

    private void SetPossibleCombinations()
    {
        var previousYearIDs = _dbGroup.Years
            .Where(x => x.Year == _dbGiftingGroupYear.Year - 1) // TODO: Try going back 2 years, then if that fails try 1
            .SelectMany(y => y.Users)
            .Where(z => z.Included)
            .ToList();

        foreach (Santa_YearGroupUser member in _participatingMembers)
        {
            AddMembersToCombinations(previousYearIDs, member);
        }
    }

    private List<GiverAndReceiverCombination> GetActualCombinations()
    {        
        bool combinationWorked = TryCalculatingCombination();

        if (!combinationWorked)
        {
            _actualCombinations = new(); // return an empty list
        }

        return _actualCombinations;
    }

    private void AddMembersToCombinations(List<Santa_YearGroupUser> previousYearIDs, Santa_YearGroupUser member)
    {
        var partnerIDs = DbContext.Santa_PartnerLinks
            .Where(p => p.ConfirmedByPartner2 && p.RelationshipEnded == null && p.SuggestedById == member.SantaUserId)
            .Select(p => p.ConfirmedById).ToList();

        var partneredIDs = DbContext.Santa_PartnerLinks
            .Where(p => p.ConfirmedByPartner2 && p.RelationshipEnded == null && p.ConfirmedById == member.SantaUserId)
            .Select(p => p.SuggestedById).ToList();

        var partners = partnerIDs.Union(partneredIDs);

        var possibleRecipients = _participatingMembers
            .Where(am => am.SantaUserId != member.SantaUserId
                && !partnerIDs.Contains(am.SantaUserId)
                && !previousYearIDs.Any(u => am.SantaUserId == u.GivingToUserId));

        foreach (var pr in possibleRecipients)
        {
            var combination = new GiverAndReceiverCombination { GiverId = member.SantaUserId, RecipientId = pr.SantaUserId };
            _possibleCombinations.Add(combination);
        }
    }

    private bool TryCalculatingCombination()
    {        
        bool canContinue = true;

        while (canContinue && _memberPosition < _participatingMembers.Count) // iterate forward through the list of members
        {
            _memberPosition++;
            Santa_YearGroupUser member = _participatingMembers[_memberPosition];
            _failedCombinations.RemoveAll(x => x.GiverId == member.SantaUserId); // just in case

            canContinue = TryCombination(member);
        }

        return true;
    }

    private bool TryCombination(Santa_YearGroupUser member)
    {        
        GiverAndReceiverCombination? combinationFound = FindWorkingCombination(member);

        if (combinationFound == null)
        {
            if (_memberPosition == 0)
            {
                return false; // either the first member has no possible combinations, or we've gone right back to the start
            }
            else // go back to the previous member in the list
            {
                _failedCombinations.RemoveAll(x => x.GiverId == member.SantaUserId); // clear this for when we can move forward again
                _actualCombinations.RemoveAll(x => x.GiverId == member.SantaUserId); // just in case

                return RetryPreviousMember();
            }
        }
        else
        {
            _actualCombinations.Add(combinationFound);
        }

        return true;
    }

    /// <summary>
    /// Wind back to the previous member, treat their previous result as a 'failure', and try a different combination
    /// </summary>
    private bool RetryPreviousMember()
    {
        _memberPosition--;
        var previousMember = _participatingMembers[_memberPosition];

        var previousFailures = _actualCombinations
            .Where(x => x.GiverId == previousMember.SantaUserId)
            .ToList(); // there should be only one, but just in case

        MoveFromActualToFailed(previousFailures);

        return TryCombination(previousMember);
    }

    private void MoveFromActualToFailed(List<GiverAndReceiverCombination> previousFailures)
    {
        _failedCombinations.AddRange(previousFailures); // avoid using that combination again (unless we wind back even further)

        foreach (GiverAndReceiverCombination failure in previousFailures) // there should be only one, but just in case
        {
            _actualCombinations.Remove(failure);
        }
    }

    private GiverAndReceiverCombination? FindWorkingCombination(Santa_YearGroupUser member)
    {
        int giverId = member.SantaUserId;

        List<int> recipintIDs = _possibleCombinations
            .Where(pc => pc.GiverId == giverId)
            .Where(x => _actualCombinations.Any(y => y.RecipientId == x.RecipientId) == false) // avoid 'taken' recipents
            .Where(x => _failedCombinations.Any(y => y.GiverId == giverId && y.RecipientId == x.RecipientId) == false) // don't repeat combinations that we had to wind back
            .Select(pc => pc.RecipientId)
            .ToList();

        if (!recipintIDs.Any())
        {
            return null;
        }
        
        Random random = new Random();
        int recipientPosition = random.Next(0, recipintIDs.Count - 1);
        int recipientId = recipintIDs[recipientPosition];

        return new GiverAndReceiverCombination { GiverId = giverId, RecipientId = recipientId };
    }
}
