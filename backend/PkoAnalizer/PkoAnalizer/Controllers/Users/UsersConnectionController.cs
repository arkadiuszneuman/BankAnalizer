using Microsoft.AspNetCore.Mvc;
using PkoAnalizer.Core.Commands.Users;
using PkoAnalizer.Core.Cqrs.Command;
using PkoAnalizer.Logic.Users.UsersConnections;
using PkoAnalizer.Logic.Users.UsersConnections.ViewModels;
using PkoAnalizer.Web.Controllers.Users.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PkoAnalizer.Web.Controllers.Users
{
    public class UsersConnectionController : BankControllerBase
    {
        private readonly ICommandsBus bus;
        private readonly UsersConnectionsReader usersConnectionsReader;

        public UsersConnectionController(ICommandsBus bus,
            UsersConnectionsReader usersConnectionsReader)
        {
            this.bus = bus;
            this.usersConnectionsReader = usersConnectionsReader;
        }

        [HttpGet]
        [Route("")]
        public Task<IEnumerable<UsersConnectionViewModel>> Get([FromQuery] UsersConnectionsReader.UsersConnectionsFilter filter) => 
            usersConnectionsReader.LoadUserConnections(filter, GetCurrentUserId());

        [HttpPost]
        [Route("")]
        public IActionResult RequestConnection(RequestConnectionViewModel requestedConnection)
        {
            var command = new RequestUserConnectionCommand(GetCurrentUserId(), requestedConnection.RequestedUsername);
            _ = bus.Send(command);
            return Accepted(command.Id);
        }

        [HttpPost]
        [Route("accept")]
        public IActionResult AcceptConnection(AcceptConnectionViewModel acceptConnection)
        {
            var command = new AcceptConnectionCommand(GetCurrentUserId(), acceptConnection.HostUserIdToAcceptConnection);
            _ = bus.Send(command);
            return Accepted(command.Id);
        }
    }
}
