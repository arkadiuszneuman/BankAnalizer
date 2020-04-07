using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PkoAnalizer.Logic.Transactions.Read;
using PkoAnalizer.Logic.Transactions.Read.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PkoAnalizer.Web.Controllers.Transaction
{
    [Route("api/transaction-type")]
    [ApiController]
    [Authorize]
    public class TransactionTypeController : BankControllerBase
    {
        private readonly TransactionReader transactionReader;

        public TransactionTypeController(TransactionReader transactionReader)
        {
            this.transactionReader = transactionReader;
        }

        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<TransactionTypeViewModel>> Index() =>
            await transactionReader.ReadTransactionTypes(GetCurrentUserId());
    }
}
