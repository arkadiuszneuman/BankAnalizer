using PkoAnalizer.Core.Cqrs.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace PkoAnalizer.Core.Commands.Users
{
    public class AcceptConnectionCommand : ICommand
    {
        public Guid Id { get; } = Guid.NewGuid();
        public Guid CurrentUserId { get; set; }
        public Guid HostUserIdToAcceptConnection { get; set; }

        public AcceptConnectionCommand(Guid currentUserId, Guid hostUserIdToAcceptConnection)
        {
            CurrentUserId = currentUserId;
            HostUserIdToAcceptConnection = hostUserIdToAcceptConnection;
        }
    }
}
