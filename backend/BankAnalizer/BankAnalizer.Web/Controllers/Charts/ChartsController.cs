using BankAnalizer.Logic.Charts.Read;
using BankAnalizer.Logic.Charts.ViewModels;
using BankAnalizer.Logic.Transactions.Read;
using BankAnalizer.Logic.Transactions.Read.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankAnalizer.Web.Controllers.Charts
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ChartsController : BankControllerBase
    {
        private readonly GroupsReader groupsReader;
        private readonly TransactionReader transactionReader;

        public ChartsController(GroupsReader groupsReader,
            TransactionReader transactionReader)
        {
            this.groupsReader = groupsReader;
            this.transactionReader = transactionReader;
        }

        [HttpGet]
        [Route("groups")]
        public async Task<IEnumerable<GroupsViewModel>> GetGroups(DateTime? dateFrom, DateTime? dateTo) =>
            await groupsReader.GetGroups(dateFrom, dateTo, GetCurrentUserId());

        [HttpGet]
        [Route("groups/transactions")]
        public IAsyncEnumerable<TransactionViewModel> GetGroupTransactions([FromQuery] TransactionsFilter transactionsFilter) =>
            transactionReader.ReadTransactions(transactionsFilter, GetCurrentUserId());
    }
}
