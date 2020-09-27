using BankAnalizer.Core.Cqrs.Command;
using System;
using BankAnalizer.Logic.Transactions.Import.Models;

namespace BankAnalizer.Logic.Transactions.Import.Commands
{
    public class ImportCommand : ICommand
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; }
        public TransactionsFile TransactionsFile { get; }

        public ImportCommand(Guid userId, TransactionsFile transactionsFile)
        {
            UserId = userId;
            TransactionsFile = transactionsFile;
        }
    }
}
