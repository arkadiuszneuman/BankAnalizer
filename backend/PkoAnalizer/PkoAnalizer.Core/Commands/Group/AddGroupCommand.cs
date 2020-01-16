using PkoAnalizer.Core.Cqrs.Command;
using System;

namespace PkoAnalizer.Core.Commands.Group
{
    public class AddGroupCommand : ICommand
    {
        public Guid Id { get; } = Guid.NewGuid();
        public Guid BankTransactionId { get; }
        public string GroupName { get; }

        public AddGroupCommand(Guid bankTransactionId, string name)
        {
            BankTransactionId = bankTransactionId;
            GroupName = name;
        }
    }
}