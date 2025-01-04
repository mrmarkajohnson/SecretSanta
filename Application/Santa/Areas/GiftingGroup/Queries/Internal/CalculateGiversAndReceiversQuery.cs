using Application.Santa.Areas.GiftingGroup.BaseModels;

namespace Application.Santa.Areas.GiftingGroup.Queries.Internal;

internal class CalculateGiversAndReceiversQuery : BaseQuery<List<GiverAndReceiverCombination>>
{
    private readonly Santa_GiftingGroupYear _dbGiftingGroupYear;
    private readonly Santa_GiftingGroup _dbGroup;
    private readonly List<Santa_YearGroupUser> _participatingMembers;
    private List<Santa_YearGroupUser> _activeCombinationsInOtherGroups = new();

    private int _previousYears = 0;
    private int _memberPosition = 0;
    private bool _ignoreOtherGroups = false;

    private List<GiverAndReceiverCombination> _possibleCombinations = new();
    private List<GiverAndReceiverCombination> _actualCombinations = new();
    private List<GiverAndReceiverCombination> _failedCombinations = new();

    public CalculateGiversAndReceiversQuery(Santa_GiftingGroupYear dbGiftingGroupYear, ref int previousYears)
    {
        _dbGiftingGroupYear = dbGiftingGroupYear;
        _dbGroup = dbGiftingGroupYear.GiftingGroup;
        _participatingMembers = dbGiftingGroupYear.ParticipatingMembers();
        _previousYears = previousYears;
    }

    protected override Task<List<GiverAndReceiverCombination>> Handle()
    {
        int? lastCombinationCount = null; // used to avoid unnecessary processing, if no improvement on last try

        List<Santa_YearGroupUser> _activeCombinationsInOtherGroups = DbContext.Santa_YearGroupUsers
            .Where(x => x.GivingToUserId != null)
            .Where(x => x.Year.Year == _dbGiftingGroupYear.Year)
            .Where(x => x.Year.GiftingGroupId != _dbGiftingGroupYear.GiftingGroupId)
            .Where(x => _participatingMembers.Any(y => y.SantaUserId == x.SantaUserId))
            .Where(x => _participatingMembers.Any(y => y.SantaUserId == x.GivingToUserId))
            .ToList();

        while (_actualCombinations.Count == 0 && (_previousYears >= 0 || _ignoreOtherGroups == false))
        {
            // first reset all of the lists (NB: if reducing previousYears, it's still quicker/easier to reset and start again)
            _possibleCombinations = new();
            _actualCombinations = new();
            _failedCombinations = new();

            SetPossibleCombinations(_previousYears); // sets the _possibleCombinations variable

            if (_possibleCombinations.Count == 0 
                || (lastCombinationCount != null && _possibleCombinations.Count <= lastCombinationCount))
            {
                ReducePreviousYearOrIgnoreOtherGroups(); // no point trying again without changing something
            }
            else
            {
                SetActualCombinations();

                if (_actualCombinations.Count == 0)
                {
                    ReducePreviousYearOrIgnoreOtherGroups(); // reduce the number of years to go back, to increase the number of possible combinations                    
                }
            }

            lastCombinationCount = _possibleCombinations.Count;
        }

        return Task.FromResult(_actualCombinations);
    }

    private void ReducePreviousYearOrIgnoreOtherGroups()
    {
        if (_previousYears == 0 && _ignoreOtherGroups == false)
        {
            _ignoreOtherGroups = true;
        }
        else
        {
            _previousYears--;
        }
    }

    private void SetPossibleCombinations(int previousYears) // TODO: Keep a count of previousYearParticipants, and if it doesn't change, stop 
    {
        var previousParticipants = _dbGroup.Years
            .Where(x => x.Year < _dbGiftingGroupYear.Year)
            .Where(x => x.Year >= _dbGiftingGroupYear.Year - previousYears)
            .SelectMany(y => y.Users)
            .Where(z => z.Included == true)
            .ToList();

        foreach (Santa_YearGroupUser member in _participatingMembers)
        {
            AddPossibleReceipientsForMember(previousParticipants, member);
        }
    }

    private void SetActualCombinations()
    {
        bool combinationWorked = TryCalculatingCombination();

        if (!combinationWorked)
        {
            _actualCombinations = new(); // return an empty list
        }
    }

    private void AddPossibleReceipientsForMember(List<Santa_YearGroupUser> previousYearParticipants, Santa_YearGroupUser member)
    {
        List<int> partnerIDs = GetPartnerIDs(member);
        List<int> recipientIDsInOtherGroups = new();

        var memberPreviousRecipientIDs = previousYearParticipants
            .Where(u => u.SantaUserId == member.SantaUserId)
            .Select(u => u.GivingToUserId)
            .ToList();

        if (_ignoreOtherGroups == false)
        {
            recipientIDsInOtherGroups = _activeCombinationsInOtherGroups
                .Where(x => x.SantaUserId == member.SantaUserId)
                .Select(x => x.GivingToUserId ?? 0)
                .ToList();
        }

        var possibleRecipients = _participatingMembers
            .Where(x => x.SantaUserId != member.SantaUserId
                && !partnerIDs.Contains(x.SantaUserId)
                && !memberPreviousRecipientIDs.Contains(x.SantaUserId)
                && !recipientIDsInOtherGroups.Contains(x.SantaUserId))
            .ToList();

        foreach (var recipient in possibleRecipients)
        {
            var combination = new GiverAndReceiverCombination { GiverId = member.SantaUserId, RecipientId = recipient.SantaUserId };
            _possibleCombinations.Add(combination);
        }
    }

    private List<int> GetPartnerIDs(Santa_YearGroupUser member)
    {
        var activePartnerLinks = DbContext.Santa_PartnerLinks
            .Where(p => p.Confirmed && p.DateDeleted == null && p.DateArchived == null
                && (p.RelationshipEnded == null || !p.SuggestedByIgnoreOld || !p.ConfirmedByIgnoreOld));

        var suggestingPartnerIDs = activePartnerLinks
            .Where(p => p.SuggestedBySantaUserId == member.SantaUserId)
            .Select(p => p.ConfirmingSantaUserId).ToList(); // relationships where this member 'suggested' the partnership

        var confirmedPartnerIDs = activePartnerLinks
            .Where(p => p.ConfirmingSantaUserId == member.SantaUserId)
            .Select(p => p.SuggestedBySantaUserId).ToList(); // relationships where this member 'confirmed' the partnership

        List<int> partnerIDs = suggestingPartnerIDs.Union(confirmedPartnerIDs).ToList();
        return partnerIDs;
    }

    private bool TryCalculatingCombination()
    {
        bool canContinue = true;

        while (canContinue && _memberPosition < _participatingMembers.Count) // iterate forward through the list of members
        {            
            Santa_YearGroupUser member = _participatingMembers[_memberPosition];
            _failedCombinations.RemoveAll(x => x.GiverId == member.SantaUserId); // just in case

            canContinue = TryCombination(member);
            _memberPosition++;
        }

        return canContinue;
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
            .Where(x => x.GiverId == giverId)
            .Where(x => _actualCombinations.Any(y => y.RecipientId == x.RecipientId) == false) // avoid 'taken' recipents
            .Where(x => _failedCombinations.Any(y => y.GiverId == giverId && y.RecipientId == x.RecipientId) == false) // don't repeat combinations that we had to wind back
            .Select(x => x.RecipientId)
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
