using AutoMapper;
using BankAnalizer.Core.Cqrs.Command;
using BankAnalizer.Logic.Rules.Logic;
using BankAnalizer.Logic.Rules.ViewModels;
using BankAnalizer.Logic.Transactions.Export;
using BankAnalizer.Logic.Transactions.Import.Commands;
using BankAnalizer.Logic.Transactions.Read;
using BankAnalizer.Logic.Transactions.Read.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using BankAnalizer.Logic.Transactions.Import.Models;

namespace BankAnalizer.Web.Controllers.Transaction
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionController : BankControllerBase
    {
        private readonly ICommandsBus bus;
        private readonly TransactionReader transactionReader;
        private readonly ColumnFinder columnFinder;
        private readonly BankTransactionRuleFinder bankTransactionRuleFinder;
        private readonly IMapper mapper;
        private readonly Export export;

        public TransactionController(ICommandsBus bus,
            TransactionReader transactionReader,
            ColumnFinder columnFinder,
            BankTransactionRuleFinder bankTransactionRuleFinder,
            IMapper mapper,
            Export export)
        {
            this.bus = bus;
            this.transactionReader = transactionReader;
            this.columnFinder = columnFinder;
            this.bankTransactionRuleFinder = bankTransactionRuleFinder;
            this.mapper = mapper;
            this.export = export;
        }

        [HttpPost]
        [Route("import")]
        public async Task<ActionResult> Import(IFormFile file)
        {
            if (file.Length < 1024 * 1024)
            {
                var transactionsFile = await ReadStream(file);
                var command = new ImportCommand(GetCurrentUserId(), transactionsFile);

                _ = bus.SendAsync(command);
                return Accepted(command);
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<TransactionsFile> ReadStream(IFormFile file)
        {
            await using var stream = file.OpenReadStream();
            return await TransactionsFile.CreateFromStream(stream);
        }

        [HttpGet]
        [Route("")]
        public IAsyncEnumerable<TransactionViewModel> Index([FromQuery] TransactionsFilter filter) =>
            transactionReader.ReadTransactions(filter, GetCurrentUserId());

        [HttpGet]
        [Route("transaction-columns")]
        public async Task<IEnumerable<ColumnViewModel>> GetTransactionColumns() =>
            await columnFinder.FindColumns(GetCurrentUserId());

        [HttpPost]
        [Route("find-transactions-from-rule")]
        public async Task<IEnumerable<TransactionViewModel>> FindTransactionsFromRule(RuleParsedViewModel rule) =>
            mapper.Map<IEnumerable<TransactionViewModel>>(await bankTransactionRuleFinder
                .FindBankTransactionsFitToRule(rule, GetCurrentUserId()));

        [HttpGet]
        [Route("export")]
        public async Task<ActionResult> Export()
        {
            var exportedJson = await export.GetExportedJson();
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(exportedJson));
            var contentType = "APPLICATION/octet-stream";
            var fileName = "something.txt";
            return File(stream, contentType, fileName);
        }
    }
}