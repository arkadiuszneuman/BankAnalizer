using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PkoAnalizer.Core.Commands.Import;
using PkoAnalizer.Core.Cqrs.Command;
using System.Collections;
using System.Collections.Generic;

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
        public ActionResult ImportAll()
        {
            var command = new ImportCommand();
            bus.Send(command);
            return Accepted(command);
        }
    }
}