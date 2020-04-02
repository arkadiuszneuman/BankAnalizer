using Microsoft.AspNetCore.Mvc;
using PkoAnalizer.Core.Commands.Group;
using PkoAnalizer.Core.Cqrs.Command;
using PkoAnalizer.Web.Controllers.Group.ViewModels;
using System;
using System.Threading.Tasks;

namespace PkoAnalizer.Web.Controllers.Group
{
    [Route("api/transaction/[controller]")]
    [ApiController]
    public class GroupController : BankControllerBase
    {
        private readonly ICommandsBus bus;

        public GroupController(ICommandsBus bus)
        {
            this.bus = bus;
        }

        [HttpDelete]
        [Route("")]
        public async Task<ActionResult> RemoveGroup(BankTransactionGroupViewModel addGroupViewModel)
        {
            var command = new RemoveGroupCommand(addGroupViewModel.BankTransactionId, addGroupViewModel.GroupName, GetCurrentUserId());
            _ = bus.SendAsync(command);
            return Accepted(command);
        }
    }
}
