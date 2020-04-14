using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAnalizer.Core.SignalR
{
    public class UsersHub : Hub
    {
        private static readonly ConcurrentDictionary<string, List<string>> registeredClients = new ConcurrentDictionary<string, List<string>>();
        private static readonly object lockObject = new object();

        public void RegisterClient(string clientId)
        {
            if (!registeredClients.ContainsKey(clientId))
                registeredClients.TryAdd(clientId, new List<string>());

            lock (lockObject)
            {
                registeredClients[clientId].Add(Context.ConnectionId);
            }
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            lock (lockObject)
            {
                foreach (var client in registeredClients)
                {
                    if (client.Value.Any(c => c == Context.ConnectionId))
                    {
                        client.Value.Remove(Context.ConnectionId);
                        return Task.CompletedTask;
                    }
                }
            }

            return Task.CompletedTask;
        }

        public static IEnumerable<string> GetRegisteredClients(string clientId)
        {
            if (!registeredClients.ContainsKey(clientId))
                return Enumerable.Empty<string>();

            return registeredClients[clientId];
        }
    }
}
