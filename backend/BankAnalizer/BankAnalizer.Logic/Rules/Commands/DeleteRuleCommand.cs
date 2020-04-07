using BankAnalizer.Core.Api;
using System;

namespace BankAnalizer.Logic.Rules.Commands
{
    public class DeleteRuleCommand : Command
    {
        public Guid RuleId { get; set; }
    }
}
