using PkoAnalizer.Core.Cqrs.Command;
using PkoAnalizer.Core.ViewModels.Rules;
using System;

namespace PkoAnalizer.Core.Commands.Rules
{
    public class SaveRuleCommand : ICommand
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string ConnectionId { get; private set; }
        public RuleParsedViewModel Rule { get; private set; }

        public SaveRuleCommand(string connectionId, RuleParsedViewModel rule)
        {
            ConnectionId = connectionId;
            Rule = rule;
        }
    }
}
