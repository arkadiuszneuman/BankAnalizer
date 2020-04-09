using BankAnalizer.Core.Cqrs.Event;
using BankAnalizer.Core.SignalR;
using BankAnalizer.Logic.Transactions.Import.Events;
using System.Threading.Tasks;

namespace BankAnalizer.Logic.Transactions.Import.EventHandlers
{
    public class CommandCompletedEventHandler : IHandleEvent<CommandCompletedEvent>
    {
        private readonly ISignalrClient signalrClient;

        public CommandCompletedEventHandler(ISignalrClient signalrClient) => this.signalrClient = signalrClient;

        public Task Handle(CommandCompletedEvent @event) =>
            signalrClient.SendNotificationToUserId("command-completed", @event.UserId.ToString(), @event);
    }
}
