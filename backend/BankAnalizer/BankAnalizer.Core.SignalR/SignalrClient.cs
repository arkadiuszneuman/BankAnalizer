using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;

namespace BankAnalizer.Core.SignalR
{
    public interface ISignalrClient
    {
        Task SendNotificationToUserId(string methodName, string userId, object notification);
    }

    public class SignalrClient : ISignalrClient
    {
        private readonly IHubContext<UsersHub> context;

        public SignalrClient(IHubContext<UsersHub> context) => this.context = context;

        public Task SendNotificationToUserId(string methodName, string userId, object notification)
        {
            var registeredClients = UsersHub.GetRegisteredClients(userId.ToString());
            return context.Clients.Clients(registeredClients.ToList()).SendAsync(methodName, notification);
        }
    }
}
