using Global.Abstractions.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Santa.Global;

public abstract class BaseQuery<TItem> : BaseRequest
{
    public abstract Task<TItem> Handle();
}
