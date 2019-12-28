using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PkoAnalizer.Core.Commands.Import;
using PkoAnalizer.Core.Cqrs.Command;
using PkoAnalizer.Db;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PkoAnalizer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly ICommandsBus bus;

        public ImportController(ICommandsBus bus)
        {
            this.bus = bus;
        }

        [HttpGet]
        public async Task<ActionResult> ImportAll([FromHeader]string connectionId)
        {
            var command = new ImportCommand(connectionId);
            _ = bus.Send(command);
            return Accepted(command);
        }
    }
}