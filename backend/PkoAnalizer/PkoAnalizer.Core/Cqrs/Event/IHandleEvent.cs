using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PkoAnalizer.Core.Cqrs.Event
{
    public interface IHandleEvent
    {
    }

    public interface IHandleEvent<TEvent> : IHandleEvent
    where TEvent : IEvent
    {
        Task Handle(TEvent @event);
    }
}
