using BankAnalizer.Core.Cqrs.Event;
using BankAnalizer.Db.Models;
using System;

namespace BankAnalizer.Logic.Groups.Events
{
    public class GroupToTransactionAddedEvent : IEvent
    {
        public BankTransaction BankTransaction { get; }
        public Group Group { get; }
        public Guid UserId { get; }

        public GroupToTransactionAddedEvent(BankTransaction bankTransaction, Group group, Guid userId)
        {
            BankTransaction = bankTransaction;
            Group = group;
            UserId = userId;
        }
    }
}
