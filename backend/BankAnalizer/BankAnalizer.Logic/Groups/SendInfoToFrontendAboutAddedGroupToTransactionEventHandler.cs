using BankAnalizer.Core.Cqrs.Event;
using BankAnalizer.Logic.Groups.Events;
using BankAnalizer.Logic.Transactions.Import.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BankAnalizer.Logic.Groups
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
