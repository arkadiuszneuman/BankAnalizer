using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PkoAnalizer.Core.Cqrs.Event
{
    public interface IEventsBus
    {
        Task Publish<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}
