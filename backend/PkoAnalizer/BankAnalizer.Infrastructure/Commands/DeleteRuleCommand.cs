using BankAnalizer.Core.Api;
using System;

namespace BankAnalizer.Infrastructure.Commands
{
    public class DeleteRuleCommand : Command
    {
        public Guid RuleId { get; set; }
    }
}
