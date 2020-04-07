using BankAnalizer.Core.Api;
using System;

namespace BankAnalizer.Logic.Groups.Commands
{
    public class RemoveGroupCommand : Command
    {
        public Guid BankTransactionId { get; set; }
        public string GroupName { get; set; }
    }
}
