using Microsoft.AspNetCore.Mvc;
using PkoAnalizer.Core.Commands.Group;
using PkoAnalizer.Core.Cqrs.Command;
using PkoAnalizer.Web.Controllers.Group.ViewModels;
using System.Threading.Tasks;

namespace PkoAnalizer.Web.Controllers.Group
{
    [Route("api/transaction/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly ICommandsBus bus;

        public GroupController(ICommandsBus bus)
        {
            this.bus = bus;
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> AddGroup(AddGroupViewModel addGroupViewModel)
        {
            var command = new AddGroupCommand(addGroupViewModel.BankTransactionId, addGroupViewModel.Name);
            _ = bus.Send(command);
            return Accepted(command);
        }
    }
}
