using BankAnalizer.Core.Cqrs.Command;
using BankAnalizer.Logic.Users.UsersConnections;
using BankAnalizer.Logic.Users.UsersConnections.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankAnalizer.Web.Controllers.Users
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
    }
}
