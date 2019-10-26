using PkoAnalizer.Core.Cqrs.Event;
using PkoAnalizer.Db.Models;
using PkoAnalizer.Logic.Import.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PkoAnalizer.Logic.Import.Events
{
    public class TransactionSavedEvent : IEvent
    {
        public PkoTransaction Transaction { get; }
        public BankTransaction DatabaseTransaction { get; }

        public TransactionSavedEvent(PkoTransaction transaction, BankTransaction databaseTransaction)
        {
            Transaction = transaction;
            DatabaseTransaction = databaseTransaction;
        }
    }
}
