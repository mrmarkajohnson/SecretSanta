using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Santa.Areas.Account.BaseModels;

public class UnHashedUserIdWithGreeting : UnHashedUserId
{
    public required string Greeting { get; set; }
}
