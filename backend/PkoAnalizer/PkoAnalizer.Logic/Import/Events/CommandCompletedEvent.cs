﻿using PkoAnalizer.Core.Cqrs.Event;
using System;

namespace PkoAnalizer.Logic.Import.Events
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