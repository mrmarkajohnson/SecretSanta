namespace Application.Areas.GiftingGroup.Queries;

public sealed class GetUniqueJoinerTokenQuery : BaseQuery<string>
{
    private readonly string? _existingToken;

    public GetUniqueJoinerTokenQuery(string? existingToken = null)
    {
        _existingToken = existingToken;
    }

    protected override Task<string> Handle()
    {
        List<string> existingTokens = DbContext.Santa_GiftingGroups.Select(x => x.JoinerToken).ToList();

        if (_existingToken.IsNotEmpty() && IsUnique(_existingToken, existingTokens))
        {
            return Result(_existingToken);
        }

        string newToken = string.Empty;
        bool isUnique = false;

        while (!isUnique)
        {
            newToken = RandomHelper.RandomAlphanumericCharacters(20);
            isUnique = IsUnique(newToken, existingTokens);
        }

        return Result(newToken);
    }

    private bool IsUnique(string joinerToken, List<string> existingTokens)
    {
        return existingTokens.Contains(joinerToken) == false;
    }
}
