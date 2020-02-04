using PkoAnalizer.Core.Cqrs.Command;
using System;

namespace PkoAnalizer.Core.Commands.Rules
{
    public class DeleteRuleCommand : ICommand
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string ConnectionId { get; private set; }
        public Guid RuleId { get; private set; }

        public DeleteRuleCommand(string connectionId, Guid ruleId)
        {
            ConnectionId = connectionId;
            RuleId = ruleId;
        }
    }
}
