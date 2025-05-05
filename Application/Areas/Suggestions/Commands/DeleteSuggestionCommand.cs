using Application.Areas.Suggestions.BaseModels;
using Application.Shared.Requests;
using Global.Extensions.Exceptions;

namespace Application.Areas.Suggestions.Commands;

public class DeleteSuggestionCommand : BaseCommand<int>
{
    public DeleteSuggestionCommand(int suggestionKey) : base(suggestionKey)
    {
    }

    protected override Task<ICommandResult<int>> HandlePostValidation()
    {
        Santa_User dbSantaUser = GetCurrentSantaUser(s => s.Suggestions);

        var suggestion = new ManageSuggestion();

        if (Item > 0)
        {
            Santa_Suggestion? dbSuggestion = dbSantaUser.Suggestions.FirstOrDefault(x => x.SuggestionKey == Item);
            
            if (dbSuggestion != null)
            {
                if (dbSuggestion.DateCreated.Year < DateTime.Now.Year && dbSuggestion.YearGroupUserLinks.Any())
                {
                    dbSuggestion.DateArchived = DateTime.Now;
                }
                else
                {
                    dbSuggestion.DateDeleted = DateTime.Now;
                }
            }
            else
            {
                throw new NotFoundException("Suggestion");
            }
        }
        else
        {
            throw new NotFoundException("Suggestion");
        }

        return SaveAndReturnSuccess();
    }
}
