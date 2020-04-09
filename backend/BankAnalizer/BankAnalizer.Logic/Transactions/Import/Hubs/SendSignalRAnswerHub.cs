using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BankAnalizer.Logic.Transactions.Import.Hubs
{
    public class SendSignalRAnswerHub : Hub
    {
        private static readonly ConcurrentDictionary<Guid, List<string>> registeredClients = new ConcurrentDictionary<Guid, List<string>>();

        public string GetConnectionId() => Context.ConnectionId;

        public void RegisterClient(Guid clientId, string connectionId)
        {
            if (registeredClients.ContainsKey(clientId))
                registeredClients[clientId].Add(connectionId);
            else
                registeredClients.TryAdd(clientId, new List<string> { connectionId });
        }

        public static List<string> GetRegisteredClients(Guid clientId)
        {
            if (!registeredClients.ContainsKey(clientId))
                return new List<string>();

            return registeredClients[clientId];
        }
    }
}
