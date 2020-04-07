using BankAnalizer.Core.Cqrs.Command;
using System;

namespace BankAnalizer.Logic.Transactions.Import.Commands
{
    public class ImportCommand : ICommand
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ConnectionId { get; set; }
        public Guid UserId { get; }
        public string FileText { get; }

        public ImportCommand(string connectionId, Guid userId, string file)
        {
            ConnectionId = connectionId;
            UserId = userId;
            FileText = file;
        }
    }
}
