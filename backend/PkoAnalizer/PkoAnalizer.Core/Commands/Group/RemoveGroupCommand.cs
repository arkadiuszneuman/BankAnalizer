using PkoAnalizer.Core.Cqrs.Command;
using System;

namespace PkoAnalizer.Core.Commands.Group
{
    public class RemoveGroupCommand : ICommand
    {
        public Guid BankTransactionId { get; set; }
        public string GroupName { get; set; }

        public RemoveGroupCommand(Guid bankTransactionId, string groupName)
        {
            BankTransactionId = bankTransactionId;
            GroupName = groupName;
        }
    }
}
