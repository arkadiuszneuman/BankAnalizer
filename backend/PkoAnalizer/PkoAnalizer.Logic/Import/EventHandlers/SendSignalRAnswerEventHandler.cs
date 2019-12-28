using Microsoft.AspNetCore.SignalR;
using PkoAnalizer.Core.Cqrs.Event;
using PkoAnalizer.Logic.Import.Events;
using PkoAnalizer.Logic.Import.Hubs;
using System.Threading.Tasks;

namespace PkoAnalizer.Logic.Import.EventHandlers
{
    public class SendSignalRAnswerEventHandler : IHandleEvent<SignalRTransactionsImported>
    {
        private readonly IHubContext<SendSignalRAnswerHub> context;

        public SendSignalRAnswerEventHandler(IHubContext<SendSignalRAnswerHub> context)
        {
            this.context = context;
        }

        public async Task Handle(SignalRTransactionsImported @event)
        {
            await context.Clients.Client(@event.ConnectionId).SendAsync("transaction-imported", @event);
        }
    }
}
