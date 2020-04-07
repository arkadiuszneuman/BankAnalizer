using BankAnalizer.Logic.Transactions.Read;
using BankAnalizer.Logic.Transactions.Read.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankAnalizer.Web.Controllers.Transaction
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
