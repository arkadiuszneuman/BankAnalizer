using Microsoft.AspNetCore.SignalR;
using PkoAnalizer.Core.Cqrs.Event;
using PkoAnalizer.Logic.Groups.Events;
using PkoAnalizer.Logic.Import.Hubs;
using System.Threading.Tasks;

namespace PkoAnalizer.Logic.Groups
{
    public class SendInfoToFrontendAboutAddedGroupToTransactionEventHandler : IHandleEvent<GroupToTransactionAddedEvent>
    {
        private readonly IHubContext<SendSignalRAnswerHub> context;

        public SendInfoToFrontendAboutAddedGroupToTransactionEventHandler(IHubContext<SendSignalRAnswerHub> context)
        {
            this.context = context;
        }

        public async Task Handle(GroupToTransactionAddedEvent @event)
        {
            var registeredClients = SendSignalRAnswerHub.GetRegisteredClients(@event.UserId);
            await context.Clients.Clients(registeredClients).SendAsync("group-to-transaction-added",
                new { BankTransactionId = @event.BankTransaction.Id, GroupName = @event.Group.Name });
        }
    }
}
