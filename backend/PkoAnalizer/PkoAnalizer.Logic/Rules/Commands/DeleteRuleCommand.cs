using BankAnalizer.Core.Api;
using System;

namespace PkoAnalizer.Logic.Rules.Commands
{
    public class DeleteRuleCommand : Command
    {
        public Guid RuleId { get; set; }
    }
}
