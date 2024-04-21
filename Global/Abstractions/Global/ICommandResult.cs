using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global.Abstractions.Global;

public interface ICommandResult<TItem>
{
    TItem Item { get; }
    bool Success { get; }
    ValidationResult Validation { get; }
}
