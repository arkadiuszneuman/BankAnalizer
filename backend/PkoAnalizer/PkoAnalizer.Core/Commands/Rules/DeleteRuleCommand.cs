using PkoAnalizer.Core.Cqrs.Command;
using System;

namespace PkoAnalizer.Core.Commands.Rules
{
    public class DeleteRuleCommand : ICommand
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string ConnectionId { get; }
        public Guid RuleId { get; }
        public Guid UserId { get; }

        public DeleteRuleCommand(string connectionId, Guid ruleId, Guid userId)
        {
            ConnectionId = connectionId;
            RuleId = ruleId;
            UserId = userId;
        }
    }
}
