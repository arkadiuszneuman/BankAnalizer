using BankAnalizer.Core.Api;
using PkoAnalizer.Core.Cqrs.Command;
using System;

namespace BankAnalizer.Infrastructure.Commands
{
    public class AddGroupCommand : Command
    {
        public Guid BankTransactionId { get; set; }
        public string GroupName { get; set; }
        public Guid RuleId { get; set; }

        private AddGroupCommand() { }

        public AddGroupCommand(Guid bankTransactionId, string groupName, Guid userId, Guid ruleId)
        {
            BankTransactionId = bankTransactionId;
            GroupName = groupName;
            UserId = userId;
            RuleId = ruleId;
        }
    }
}