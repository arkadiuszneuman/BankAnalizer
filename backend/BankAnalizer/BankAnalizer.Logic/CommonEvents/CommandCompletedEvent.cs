using BankAnalizer.Core.Api;
using BankAnalizer.Core.Cqrs.Event;
using System;

namespace BankAnalizer.Logic.CommonEvents
{
    public class CommandCompletedEvent : IEvent
    {
        public Guid UserId { get; }
        public Guid CommandId { get; }
        public object Object { get; }

        public CommandCompletedEvent(Guid userId, Guid id, object @object = null)
        {
            UserId = userId;
            CommandId = id;
            Object = @object;
        }

        public CommandCompletedEvent(Command command, object @object = null)
        {
            UserId = command.UserId;
            CommandId = command.CommandId;
            Object = @object;
        }
    }
}
