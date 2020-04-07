using BankAnalizer.Core.Cqrs.Event;
using BankAnalizer.Logic.Transactions.Import.Events;
using BankAnalizer.Logic.Transactions.Import.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BankAnalizer.Logic.Transactions.Import.EventHandlers
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
