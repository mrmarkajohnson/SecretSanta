using Application.Areas.GiftingGroup.BaseModels;

namespace Application.Areas.GiftingGroup.Queries.Internal;

internal class CalculateGiversAndReceiversQuery : BaseQuery<List<GiverAndReceiverCombination>>
{
    private readonly Santa_GiftingGroupYear _dbGiftingGroupYear;
    private readonly Santa_GiftingGroup _dbGiftingGroup;
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
        _dbGiftingGroup = dbGiftingGroupYear.GiftingGroup;
        _participatingMembers = dbGiftingGroupYear.ParticipatingMembers();
        _previousYears = previousYears;
    }

    protected override Task<List<GiverAndReceiverCombination>> Handle()
    {
        int? lastCombinationCount = null; // used to avoid unnecessary processing, if no improvement on last try
        List<int> participatingMemberKeys = _participatingMembers.Select(y => y.SantaUserKey).ToList();

        List<Santa_YearGroupUser> _activeCombinationsInOtherGroups = DbContext.Santa_YearGroupUsers
            .Where(x => x.RecipientSantaUserKey != null)
            .Where(x => x.GiftingGroupYearKey == _dbGiftingGroupYear.GiftingGroupYearKey)
            .Where(x => x.GiftingGroupYear.GiftingGroupKey != _dbGiftingGroupYear.GiftingGroupKey)
            .Where(x => participatingMemberKeys.Contains(x.SantaUserKey))
            .Where(x => x.RecipientSantaUserKey != null && participatingMemberKeys.Contains(x.RecipientSantaUserKey.Value))
            .ToList();

        while (_actualCombinations.Count == 0 && (_previousYears >= 0 || _ignoreOtherGroups == false))
        {
            // first reset all of the lists (NB: if reducing previousYears, it's still quicker/easier to reset and start again)
            _possibleCombinations = new();
            _actualCombinations = new();
            _failedCombinations = new();

            SetPossibleCombinations(_previousYears); // sets the _possibleCombinations variable

            if (_possibleCombinations.Count == 0
                || lastCombinationCount != null && _possibleCombinations.Count <= lastCombinationCount)
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

        return Result(_actualCombinations);
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
        var previousParticipants = _dbGiftingGroup.Years
            .Where(x => x.CalendarYear < _dbGiftingGroupYear.CalendarYear)
            .Where(x => x.CalendarYear >= _dbGiftingGroupYear.CalendarYear - previousYears)
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
        List<int> partnerSantaUserKeys = GetPartnerSantaUserKeys(member);
        List<int> recipientSantaUserKeysInOtherGroups = new();

        var memberPreviousRecipientSantaUserKeys = previousYearParticipants
            .Where(u => u.SantaUserKey == member.SantaUserKey)
            .Select(u => u.RecipientSantaUserKey)
            .ToList();

        if (_ignoreOtherGroups == false)
        {
            recipientSantaUserKeysInOtherGroups = _activeCombinationsInOtherGroups
                .Where(x => x.SantaUserKey == member.SantaUserKey)
                .Select(x => x.RecipientSantaUserKey ?? 0)
                .ToList();
        }

        var possibleRecipients = _participatingMembers
            .Where(x => x.SantaUserKey != member.SantaUserKey
                && !partnerSantaUserKeys.Contains(x.SantaUserKey)
                && !memberPreviousRecipientSantaUserKeys.Contains(x.SantaUserKey)
                && !recipientSantaUserKeysInOtherGroups.Contains(x.SantaUserKey))
            .ToList();

        foreach (var recipient in possibleRecipients)
        {
            var combination = new GiverAndReceiverCombination { GiverSantaUserKey = member.SantaUserKey, RecipientSantaUserKey = recipient.SantaUserKey };
            _possibleCombinations.Add(combination);
        }
    }

    private List<int> GetPartnerSantaUserKeys(Santa_YearGroupUser member)
    {
        var allActivePartnerLinks = DbContext.Santa_PartnerLinks
            .Where(p => !p.ExchangeGifts && p.DateDeleted == null && p.DateArchived == null);

        var suggestingPartnerSantaUserKeys = allActivePartnerLinks
            .Where(p => p.SuggestedBySantaUserKey == member.SantaUserKey)
            .Select(p => p.ConfirmingSantaUserKey).ToList(); // relationships where this member 'suggested' the partnership

        var confirmedPartnerSantaUserKeys = allActivePartnerLinks
            .Where(p => p.ConfirmingSantaUserKey == member.SantaUserKey)
            .Select(p => p.SuggestedBySantaUserKey).ToList(); // relationships where this member 'confirmed' the partnership

        List<int> partnerSantaUserKeys = suggestingPartnerSantaUserKeys.Union(confirmedPartnerSantaUserKeys).ToList();
        return partnerSantaUserKeys;
    }

    private bool TryCalculatingCombination()
    {
        bool canContinue = true;

        while (canContinue && _memberPosition < _participatingMembers.Count) // iterate forward through the list of members
        {
            Santa_YearGroupUser member = _participatingMembers[_memberPosition];
            _failedCombinations.RemoveAll(x => x.GiverSantaUserKey == member.SantaUserKey); // just in case

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
                _failedCombinations.RemoveAll(x => x.GiverSantaUserKey == member.SantaUserKey); // clear this for when we can move forward again
                _actualCombinations.RemoveAll(x => x.GiverSantaUserKey == member.SantaUserKey); // just in case

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
            .Where(x => x.GiverSantaUserKey == previousMember.SantaUserKey)
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
        int giverSantaUserKey = member.SantaUserKey;

        List<int> recipientSantaUserKeys = _possibleCombinations
            .Where(x => x.GiverSantaUserKey == giverSantaUserKey)
            .Where(x => _actualCombinations.Any(y => y.RecipientSantaUserKey == x.RecipientSantaUserKey) == false) // avoid 'taken' recipents
            .Where(x => _failedCombinations.Any(y => y.GiverSantaUserKey == giverSantaUserKey && y.RecipientSantaUserKey == x.RecipientSantaUserKey) == false) // don't repeat combinations that we had to wind back
            .Select(x => x.RecipientSantaUserKey)
            .ToList();

        if (!recipientSantaUserKeys.Any())
        {
            return null;
        }
        else if (recipientSantaUserKeys.Count == 1)
        {
            return new GiverAndReceiverCombination { GiverSantaUserKey = giverSantaUserKey, RecipientSantaUserKey = recipientSantaUserKeys[0] };
        }
        else
        {
            Random random = new Random();
            int recipientPosition = random.Next(0, recipientSantaUserKeys.Count - 1);
            int recipientSantaUserKey = recipientSantaUserKeys[recipientPosition];

            return new GiverAndReceiverCombination { GiverSantaUserKey = giverSantaUserKey, RecipientSantaUserKey = recipientSantaUserKey };
        }
    }
}
