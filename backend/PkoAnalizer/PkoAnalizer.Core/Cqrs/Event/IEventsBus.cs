using System;
using System.Collections.Generic;
using System.Text;

namespace PkoAnalizer.Core.Cqrs.Event
{
    public interface IEventsBus
    {
        void Publish<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}
