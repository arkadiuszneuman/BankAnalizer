using Microsoft.AspNetCore.Mvc;
using PkoAnalizer.Core.Commands.Import;
using PkoAnalizer.Core.Cqrs.Command;
using PkoAnalizer.Logic.Read.Transactions;
using PkoAnalizer.Logic.Read.Transactions.ViewModels;
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
        private readonly ColumnFinder columnFinder;

        public TransactionController(ICommandsBus bus,
            TransactionReader transactionReader,
            ColumnFinder columnFinder)
        {
            this.bus = bus;
            this.transactionReader = transactionReader;
            this.columnFinder = columnFinder;
        }

        [HttpGet]
        [Route("import")]
        public ActionResult Import([FromHeader]string connectionId)
        {
            var command = new ImportCommand(connectionId);
            _ = bus.Send(command);
            return Accepted(command);
        }

        [HttpGet]
        [Route("")]
        public IAsyncEnumerable<TransactionViewModel> Index([FromQuery] TransactionsFilter filter) =>
            transactionReader.ReadTransactions(filter);

        [HttpGet]
        [Route("transaction-columns")]
        public async Task<IEnumerable<ColumnViewModel>> GetTransactionColumns() =>
            await columnFinder.FindColumns();
    }
}