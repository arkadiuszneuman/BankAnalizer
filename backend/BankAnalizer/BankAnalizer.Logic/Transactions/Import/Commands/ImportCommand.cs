using BankAnalizer.Core.Cqrs.Command;
using System;

namespace BankAnalizer.Logic.Transactions.Import.Commands
{
    public class ImportCommand : ICommand
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; }
        public string FileText { get; }

        public ImportCommand(Guid userId, string file)
        {
            UserId = userId;
            FileText = file;
        }
    }
}
