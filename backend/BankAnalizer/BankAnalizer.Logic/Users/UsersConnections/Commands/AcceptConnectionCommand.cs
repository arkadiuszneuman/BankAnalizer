using BankAnalizer.Core.Api;
using System;

namespace BankAnalizer.Logic.Users.UsersConnections.Commands
{
    public class AcceptConnectionCommand : Command
    {
        public Guid HostUserIdToAcceptConnection { get; set; }
    }
}
