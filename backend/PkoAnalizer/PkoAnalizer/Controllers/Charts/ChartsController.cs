using Microsoft.AspNetCore.Mvc;
using PkoAnalizer.Logic.Read.Charts.Groups;
using PkoAnalizer.Logic.Read.Transactions;
using PkoAnalizer.Logic.Read.Transactions.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PkoAnalizer.Web.Controllers.Charts
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
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
            await groupsReader.GetGroups(dateFrom, dateTo);

        [HttpGet]
        [Route("groups/transactions")]
        public IAsyncEnumerable<TransactionViewModel> GetGroupTransactions(string groupName) =>
            transactionReader.ReadTransactions(new TransactionsFilter() { GroupName = groupName });
    }
}
