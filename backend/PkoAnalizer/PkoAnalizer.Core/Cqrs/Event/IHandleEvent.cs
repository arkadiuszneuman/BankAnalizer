using System;
using System.Collections.Generic;
using System.Text;

namespace PkoAnalizer.Core.Cqrs.Event
{
    public interface IHandleEvent
    {
    }

    public interface IHandleEvent<TEvent> : IHandleEvent
    where TEvent : IEvent
    {
        void Handle(TEvent @event);
    }
}
