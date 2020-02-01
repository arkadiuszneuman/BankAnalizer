using Microsoft.AspNetCore.SignalR;
using PkoAnalizer.Core.Cqrs.Event;
using PkoAnalizer.Logic.Import.Events;
using PkoAnalizer.Logic.Import.Hubs;
using System.Threading.Tasks;

namespace PkoAnalizer.Logic.Import.EventHandlers
{
    public class CommandCompletedEventHandler : IHandleEvent<CommandCompletedEvent>
    {
        private readonly IHubContext<SendSignalRAnswerHub> context;

        public CommandCompletedEventHandler(IHubContext<SendSignalRAnswerHub> context)
        {
            this.context = context;
        }

        public async Task Handle(CommandCompletedEvent @event)
        {
            await context.Clients.Client(@event.ConnectionId).SendAsync("command-completed", @event);
        }
    }
}
