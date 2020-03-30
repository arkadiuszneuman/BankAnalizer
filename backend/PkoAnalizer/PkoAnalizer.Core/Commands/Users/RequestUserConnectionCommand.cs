using PkoAnalizer.Core.Cqrs.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace PkoAnalizer.Core.Commands.Users
{
    public class RequestUserConnectionCommand : ICommand
    {
        public Guid Id { get; } = Guid.NewGuid();
        public Guid RequestingUserId { get; set; }
        public string RequestedConnectionToUsername { get; set; }

        public RequestUserConnectionCommand(Guid requestingUserId, string requestedConnectionToUserName)
        {
            RequestingUserId = requestingUserId;
            RequestedConnectionToUsername = requestedConnectionToUserName;
        }
    }
}
