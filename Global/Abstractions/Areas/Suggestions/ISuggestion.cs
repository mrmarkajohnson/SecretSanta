using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global.Abstractions.Areas.Suggestions;

public interface ISuggestion : ISuggestionBase
{
    IEnumerable<ISuggestionYearGroupUserLink> YearGroupUserLinks { get; }
}
