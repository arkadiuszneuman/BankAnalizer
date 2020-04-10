using BankAnalizer.Core.Cqrs.Event;
using BankAnalizer.Core.SignalR;
using System.Threading.Tasks;

namespace BankAnalizer.Logic.CommonEvents.Handlers
{
    public class SignalRErrorEventHandler : IHandleEvent<ErrorEvent>
    {
        private readonly ISignalrClient signalrClient;

        public SignalRErrorEventHandler(ISignalrClient signalrClient) => this.signalrClient = signalrClient;

        public Task Handle(ErrorEvent @event) =>
            signalrClient.SendNotificationToUserId("command-error", @event.UserId.ToString(), @event);
    }
}
