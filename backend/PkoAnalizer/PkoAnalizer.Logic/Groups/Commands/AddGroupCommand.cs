using BankAnalizer.Core.Api;
using System;

namespace PkoAnalizer.Logic.Groups.Commands
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