using BankAnalizer.Core.Cqrs.Event;
using System.Threading.Tasks;

namespace BankAnalizer.Core.SignalR.Cqrs.Events.Handlers
{
    public class SignalrCommandExceptionEventHandler : IHandleEvent<CommandExceptionEvent>
    {
        private readonly ISignalrClient signalrClient;

        public SignalrCommandExceptionEventHandler(ISignalrClient signalrClient) => this.signalrClient = signalrClient;

        public Task Handle(CommandExceptionEvent @event) =>
            signalrClient.SendNotificationToUserId("command-error", @event.UserId.ToString(), new { @event.CommandId, @event.ErrorMessage });
    }
}
