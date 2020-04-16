using BankAnalizer.Core.ExtensionMethods;
using BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters;
using BankAnalizer.Logic.Transactions.Import.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Dynamic;

namespace BankAnalizer.Logic.Transactions.Import.Importers.Ing.TypeImporters
{
    public class StandardIngImporter : IIngTypeImporter
    {
        private readonly ILogger<StandardIngImporter> logger;

        public StandardIngImporter(ILogger<StandardIngImporter> logger)
        {
            this.logger = logger;
        }

        public PkoTransaction Import(string[] splittedLine)
        {
            if (splittedLine.Length < 7)
                return null;

            var transactionType = ConvertTransactionType(splittedLine.Index(6));
            if (transactionType == null)
            {
                logger.LogWarning("Unhandled type {type}", splittedLine.Index(6));
                return null;
            }

            if (string.IsNullOrEmpty(splittedLine.Index(1)))
                return null;

            return new PkoTransaction
            {
                TransactionDate = splittedLine.Index(0).ConvertToDate(),
                OperationDate = splittedLine.Index(1).ConvertToDate(),
                TransactionType = transactionType.Value.ToString(),
                Amount = splittedLine.Index(8).ConvertToDecimal(),
                Currency = splittedLine.Index(9),
                Title = splittedLine.Index(3),
                Extensions = GetExtensions(splittedLine).ToJson()
            };
        }

        private static object GetExtensions(string[] splittedLine)
        {
            dynamic extensions = new ExpandoObject();
            extensions.ContractorName = splittedLine.Index(2);

            var billNumber = splittedLine.Index(4).Trim('\'').Trim();
            if (!string.IsNullOrEmpty(billNumber))
                extensions.BillNumber = billNumber;

            var bankName = splittedLine.Index(5).Trim();
            if (!string.IsNullOrEmpty(bankName))
                extensions.BankName = bankName;

            return extensions;
        }

        private static TransactionTypeEnum? ConvertTransactionType(string transactionType) =>
            transactionType.ToUpper() switch
        {
            _ when transactionType.StartsWith("TR.KART") => TransactionTypeEnum.CardTransaction,
            _ when transactionType.StartsWith("TR.BLIK") => TransactionTypeEnum.Blik,
            _ when transactionType.StartsWith("PRZELEW") => TransactionTypeEnum.Transfer,
            _ when transactionType.StartsWith("ST.ZLEC") => TransactionTypeEnum.StandingOrder,
            _ => null
        };
    }
}
