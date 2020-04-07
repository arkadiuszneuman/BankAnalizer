using BankAnalizer.Core.Api;
using System;

namespace PkoAnalizer.Logic.Users.UsersConnections.Commands
{
    public class AcceptConnectionCommand : Command
    {
        public Guid HostUserIdToAcceptConnection { get; set; }
    }
}
