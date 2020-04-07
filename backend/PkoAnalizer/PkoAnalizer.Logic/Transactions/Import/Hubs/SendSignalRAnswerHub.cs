using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;

namespace PkoAnalizer.Logic.Transactions.Import.Hubs
{
    public class SendSignalRAnswerHub : Hub
    {
        private static readonly Dictionary<Guid, List<string>> registeredClients = new Dictionary<Guid, List<string>>();

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        public void RegisterClient(Guid clientId, string connectionId)
        {
            if (registeredClients.ContainsKey(clientId))
                registeredClients[clientId].Add(connectionId);
            else
                registeredClients.Add(clientId, new List<string> { connectionId });
        }

        public static List<string> GetRegisteredClients(Guid clientId)
        {
            if (!registeredClients.ContainsKey(clientId))
                return new List<string>();

            return registeredClients[clientId];
        }
    }
}
