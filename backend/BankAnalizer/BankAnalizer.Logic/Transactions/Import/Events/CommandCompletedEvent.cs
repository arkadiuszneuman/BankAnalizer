using BankAnalizer.Core.Cqrs.Event;
using System;

namespace BankAnalizer.Logic.Transactions.Import.Events
{
    public class CommandCompletedEvent : IEvent
    {
        public string ConnectionId { get; }
        public Guid Id { get; }
        public object Object { get; }

        public CommandCompletedEvent(string connectionId, Guid id, object @object = null)
        {
            ConnectionId = connectionId;
            Id = id;
            Object = @object;
        }
    }
}
