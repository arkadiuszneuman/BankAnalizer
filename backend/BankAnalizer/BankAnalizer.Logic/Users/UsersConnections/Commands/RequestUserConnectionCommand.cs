using BankAnalizer.Core.Api;

namespace BankAnalizer.Logic.Users.UsersConnections.Commands
{
    public class RequestUserConnectionCommand : Command
    {
        public string RequestedUsername { get; set; }
    }
}
