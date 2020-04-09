using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BankAnalizer.Logic.Transactions.Import.Hubs
{
    public class SendSignalRAnswerHub : Hub
    {
        private static readonly ConcurrentDictionary<Guid, ConcurrentBag<string>> registeredClients = new ConcurrentDictionary<Guid, ConcurrentBag<string>>();

        public void RegisterClient(Guid clientId)
        {
            if (!registeredClients.ContainsKey(clientId))
                registeredClients.TryAdd(clientId, new ConcurrentBag<string>());

            registeredClients[clientId].Add(Context.ConnectionId);
        }

        public static IEnumerable<string> GetRegisteredClients(Guid clientId)
        {
            if (!registeredClients.ContainsKey(clientId))
                return new List<string>();

            return registeredClients[clientId];
        }
    }
}
