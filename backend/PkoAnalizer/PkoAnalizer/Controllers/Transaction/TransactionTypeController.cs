using Microsoft.AspNetCore.Mvc;
using PkoAnalizer.Logic.Read.Transactions;
using PkoAnalizer.Logic.Read.Transactions.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PkoAnalizer.Web.Controllers.Transaction
{
    [Route("api/transaction-type")]
    [ApiController]
    public class TransactionTypeController : ControllerBase
    {
        private readonly TransactionReader transactionReader;

        public TransactionTypeController(TransactionReader transactionReader)
        {
            this.transactionReader = transactionReader;
        }

        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<TransactionTypeViewModel>> Index() =>
            await transactionReader.ReadTransactionTypes();
    }
}
