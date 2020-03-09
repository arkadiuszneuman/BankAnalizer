using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PkoAnalizer.Core.Commands.Import;
using PkoAnalizer.Core.Cqrs.Command;
using PkoAnalizer.Core.ViewModels.Rules;
using PkoAnalizer.Logic.Export;
using PkoAnalizer.Logic.Read.Transactions;
using PkoAnalizer.Logic.Read.Transactions.ViewModels;
using PkoAnalizer.Logic.Rules.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PkoAnalizer.Web.Controllers.Transaction
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionController : ControllerBase
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
        public async Task<ActionResult> Import([FromHeader]string connectionId,[FromHeader]Guid userId, IFormFile file)
        {
            if (file.Length < 1024 * 1024)
            {
                var text = await ReadStream(file);
                var command = new ImportCommand(connectionId, userId, text);

                _ = bus.Send(command);
                return Accepted(command);
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<string> ReadStream(IFormFile file)
        {
            using var reader = new StreamReader(file.OpenReadStream(), Encoding.GetEncoding(1250));
            var result = await reader.ReadToEndAsync();

            return result;
        }

        [HttpGet]
        [Route("")]
        public IAsyncEnumerable<TransactionViewModel> Index([FromQuery] TransactionsFilter filter) =>
            transactionReader.ReadTransactions(filter);

        [HttpGet]
        [Route("transaction-columns")]
        public async Task<IEnumerable<ColumnViewModel>> GetTransactionColumns() =>
            await columnFinder.FindColumns();

        [HttpPost]
        [Route("find-transactions-from-rule")]
        public async Task<IEnumerable<TransactionViewModel>> FindTransactionsFromRule(RuleParsedViewModel rule) =>
            mapper.Map<IEnumerable<TransactionViewModel>>(await bankTransactionRuleFinder.FindBankTransactionsFitToRule(rule));

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