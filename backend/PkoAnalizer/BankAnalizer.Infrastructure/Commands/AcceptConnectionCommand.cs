using BankAnalizer.Core.Api;
using System;

namespace BankAnalizer.Infrastructure.Commands
{
    public class AcceptConnectionCommand : Command
    {
        public Guid HostUserIdToAcceptConnection { get; set; }
    }
}
