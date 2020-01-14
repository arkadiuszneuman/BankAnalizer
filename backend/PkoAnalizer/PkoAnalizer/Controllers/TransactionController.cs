using Microsoft.AspNetCore.Mvc;
using PkoAnalizer.Core.Commands.Import;
using PkoAnalizer.Core.Cqrs.Command;
using System.Threading.Tasks;

namespace PkoAnalizer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ICommandsBus bus;

        public TransactionController(ICommandsBus bus)
        {
            this.bus = bus;
        }

        [HttpGet]
        [Route("import")]
        public async Task<ActionResult> Import([FromHeader]string connectionId)
        {
            var command = new ImportCommand(connectionId);
            _ = bus.Send(command);
            return Accepted(command);
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> Index()
        {
            return Accepted();
        }
    }
}