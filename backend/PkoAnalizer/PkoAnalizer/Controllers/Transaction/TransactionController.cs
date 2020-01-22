using Microsoft.AspNetCore.Mvc;
using PkoAnalizer.Core.Commands.Import;
using PkoAnalizer.Core.Cqrs.Command;
using PkoAnalizer.Logic.Read.Transactions;
using PkoAnalizer.Logic.Read.Transactions.ViewModels;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PkoAnalizer.Web.Controllers.Transaction
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ICommandsBus bus;
        private readonly TransactionReader transactionReader;

        public TransactionController(ICommandsBus bus,
            TransactionReader transactionReader)
        {
            this.bus = bus;
            this.transactionReader = transactionReader;
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
        public async Task<IEnumerable<TransactionViewModel>> Index() => 
            await transactionReader.ReadTransactions();
    }
}