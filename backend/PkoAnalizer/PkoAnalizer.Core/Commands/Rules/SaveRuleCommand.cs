using PkoAnalizer.Core.Cqrs.Command;
using PkoAnalizer.Core.ViewModels.Rules;
using System;

namespace PkoAnalizer.Core.Commands.Rules
{
    public class SaveRuleCommand : ICommand
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string ConnectionId { get; }
        public Guid UserId { get; }
        public RuleParsedViewModel Rule { get; }

        public SaveRuleCommand(string connectionId, Guid userId, RuleParsedViewModel rule)
        {
            ConnectionId = connectionId;
            UserId = userId;
            Rule = rule;
        }
    }
}
