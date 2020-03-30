using PkoAnalizer.Core;
using System;

namespace PkoAnalizer.Logic.Users.UsersConnections.Exceptions
{
    public class ConnectionDoesNotExistsException : BankAnalizerException
    {
        public ConnectionDoesNotExistsException(Guid userRequestingConnectionId, Guid userRequestedToConnectId) :
            base($"User connection does not exists for user requesting (host) { userRequestingConnectionId } " +
                $"and user requested { userRequestedToConnectId }")
        {

        }
    }
}
