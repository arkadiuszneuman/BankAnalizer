using BankAnalizer.Core.Cqrs.Event;
using BankAnalizer.Core.SignalR;
using BankAnalizer.Logic.Groups.Events;
using System.Threading.Tasks;

namespace BankAnalizer.Logic.Groups
{
    public class SendInfoToFrontendAboutAddedGroupToTransactionEventHandler : IHandleEvent<GroupToTransactionAddedEvent>
    {
        private readonly ISignalrClient signalrClient;

        public SendInfoToFrontendAboutAddedGroupToTransactionEventHandler(ISignalrClient signalrClient)
        {
            this.signalrClient = signalrClient;
        }

        public Task Handle(GroupToTransactionAddedEvent @event) =>
            signalrClient.SendNotificationToUserId("group-to-transaction-added", @event.UserId.ToString(),
                new
                {
                    BankTransactionId = @event.BankTransaction.Id,
                    GroupName = @event.Group.Name
                });
    }
}
