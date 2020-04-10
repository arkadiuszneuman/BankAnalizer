using BankAnalizer.Core.Cqrs.Event;
using BankAnalizer.Core.SignalR;
using BankAnalizer.Logic.Transactions.Import.Events;
using System.Threading.Tasks;

namespace BankAnalizer.Logic.CommonEvents.Handlers
{
    public class SignalRCommandCompletedEventHandler : IHandleEvent<CommandCompletedEvent>
    {
        private readonly ISignalrClient signalrClient;

        public SignalRCommandCompletedEventHandler(ISignalrClient signalrClient) => this.signalrClient = signalrClient;

        public Task Handle(CommandCompletedEvent @event) =>
            signalrClient.SendNotificationToUserId("command-completed", @event.UserId.ToString(), @event);
    }
}
