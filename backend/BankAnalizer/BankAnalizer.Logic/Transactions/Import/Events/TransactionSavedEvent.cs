using BankAnalizer.Core.Cqrs.Event;
using BankAnalizer.Db.Models;
using BankAnalizer.Logic.Transactions.Import.Models;
using System;

namespace BankAnalizer.Logic.Transactions.Import.Events
{
    public class TransactionSavedEvent : IEvent
    {
        public Guid UserId { get; }
        public PkoTransaction Transaction { get; }
        public BankTransaction DatabaseTransaction { get; }

        public TransactionSavedEvent(Guid userId, PkoTransaction transaction, BankTransaction databaseTransaction)
        {
            UserId = userId;
            Transaction = transaction;
            DatabaseTransaction = databaseTransaction;
        }
    }
}
