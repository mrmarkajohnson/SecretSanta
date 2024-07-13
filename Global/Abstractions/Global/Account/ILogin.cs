using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global.Abstractions.Global.Account;

public interface ILogin
{
    string EmailOrUserName { get; set; }
    string Password { get; set; }
    bool RememberMe { get; set; }
}
