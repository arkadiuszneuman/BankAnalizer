using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PkoAnalizer.Core.Cqrs.Event
{
    public class EventsBus : IEventsBus
    {
        private readonly Func<Type, IEnumerable<IHandleEvent>> _handlersFactory;
        public EventsBus(Func<Type, IEnumerable<IHandleEvent>> handlersFactory)
        {
            _handlersFactory = handlersFactory;
        }

        public async Task Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var handlers = _handlersFactory(typeof(TEvent))
                .Cast<IHandleEvent<TEvent>>();

            foreach (var handler in handlers)
            {
                await Task.Factory.StartNew(async () => await handler.Handle(@event));
            }
        }
    }
}
