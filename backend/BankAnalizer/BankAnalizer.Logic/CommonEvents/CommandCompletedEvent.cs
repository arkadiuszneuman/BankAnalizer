using BankAnalizer.Core.Cqrs.Event;
using System;

namespace BankAnalizer.Logic.CommonEvents
{
    public class CommandCompletedEvent : IEvent
    {
        public Guid UserId { get; }
        public Guid Id { get; }
        public object Object { get; }

        public CommandCompletedEvent(Guid userId, Guid id, object @object = null)
        {
            UserId = userId;
            Id = id;
            Object = @object;
        }
    }
}
