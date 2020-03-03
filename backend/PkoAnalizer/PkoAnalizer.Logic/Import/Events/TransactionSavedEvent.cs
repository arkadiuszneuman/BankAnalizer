using PkoAnalizer.Core.Cqrs.Event;
using PkoAnalizer.Db.Models;
using PkoAnalizer.Logic.Import.Models;
using System;

namespace PkoAnalizer.Logic.Import.Events
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
