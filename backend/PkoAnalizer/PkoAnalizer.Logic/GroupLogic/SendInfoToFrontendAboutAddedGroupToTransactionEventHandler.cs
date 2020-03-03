using Microsoft.AspNetCore.SignalR;
using PkoAnalizer.Core.Cqrs.Event;
using PkoAnalizer.Logic.GroupLogic.Events;
using PkoAnalizer.Logic.Import.Hubs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PkoAnalizer.Logic.GroupLogic
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
