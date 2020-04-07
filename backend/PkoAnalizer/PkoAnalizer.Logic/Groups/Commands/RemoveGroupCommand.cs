using BankAnalizer.Core.Api;
using PkoAnalizer.Core.Cqrs.Command;
using System;

namespace PkoAnalizer.Logic.Groups.Commands
{
    public class RemoveGroupCommand : Command
    {
        public Guid BankTransactionId { get; set; }
        public string GroupName { get; set; }
    }
}
