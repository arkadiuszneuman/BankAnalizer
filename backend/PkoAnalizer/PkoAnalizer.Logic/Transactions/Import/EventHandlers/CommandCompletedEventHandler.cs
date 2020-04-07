using Microsoft.AspNetCore.SignalR;
using PkoAnalizer.Core.Cqrs.Event;
using PkoAnalizer.Logic.Transactions.Import.Events;
using PkoAnalizer.Logic.Transactions.Import.Hubs;
using System.Threading.Tasks;

namespace PkoAnalizer.Logic.Transactions.Import.EventHandlers
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
