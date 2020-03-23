using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PkoAnalizer.Core.Commands.Users;
using PkoAnalizer.Core.Cqrs.Command;
using PkoAnalizer.Web.Controllers.Users.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PkoAnalizer.Web.Controllers.Users
{
    public class UsersConnectionController : BankControllerBase
    {
        private readonly ICommandsBus bus;

        public UsersConnectionController(ICommandsBus bus)
        {
            this.bus = bus;
        }

        [HttpPost]
        [Route("request-connection")]
        public IActionResult RequestConnection(RequestConnectionViewModel requestedConnection)
        {
            var command = new RequestUserConnectionCommand(GetCurrentUserId(), requestedConnection.RequestedUsername);
            _ = bus.Send(command);
            return Accepted(command.Id);
        }
    }
}
