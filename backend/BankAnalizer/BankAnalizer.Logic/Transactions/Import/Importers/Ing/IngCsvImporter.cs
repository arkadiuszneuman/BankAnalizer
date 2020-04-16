using BankAnalizer.Logic.Transactions.Import.Importers.Ing.TypeImporters;
using BankAnalizer.Logic.Transactions.Import.Importers.Pko;
using BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters;
using BankAnalizer.Logic.Transactions.Import.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAnalizer.Logic.Transactions.Import.Importers.Ing
{
    public class IngCsvImporter : IImporter
    {
        private readonly ILogger<IngCsvImporter> logger;
        private readonly IEnumerable<IIngTypeImporter> typeImporters;

        public IngCsvImporter(ILogger<IngCsvImporter> logger,
            IEnumerable<IIngTypeImporter> typeImporters)
        {
            this.logger = logger;
            this.typeImporters = typeImporters;
        }

        public IEnumerable<ImportedBankTransaction> ImportTransactions(string textToImport, int lastOrder)
        {
            if (!textToImport.StartsWith("\"Lista transakcji\""))
                return Enumerable.Empty<ImportedBankTransaction>();

            logger.LogInformation("Importing csv by ING importer");

            return textToImport.Split('\n')
                .RemoveEverythingBeforeHeaderWithHeaderIncluded()
                .Reverse()
                .RemoveEmptyEntries()
                .Select(line =>
                    line.SplitCsv()
                        .RemoveWhitespacesAndQuotes()
                        .ToArray())
                .ImportBy(typeImporters)
                .OnlyExistsingTransactions()
                .OrderBy(o => o.TransactionDate)
                .AssignOrder(lastOrder);
        }
    }

    public static class Pipe
    {
        public static IEnumerable<ImportedBankTransaction> ImportBy(this IEnumerable<string[]> lines, IEnumerable<IIngTypeImporter> typeImporters)
        {
            var list = new List<ImportedBankTransaction>(lines.Count());
            Parallel.ForEach(lines, splittedLine =>
            {
                ImportedBankTransaction importedTransaction = null;

                if (splittedLine.Any())
                {
                    foreach (var typeImporter in typeImporters)
                    {
                        var transaction = typeImporter.Import(splittedLine);
                        if (transaction != null)
                        {
                            if (importedTransaction != null)
                                throw new ImportException($"Too many importers for type {splittedLine.Index(2)}");

                            importedTransaction = transaction;
                        }
                    }
                }

                list.Add(importedTransaction);
            });

            return list;
        }

        public static IEnumerable<string> RemoveEverythingBeforeHeaderWithHeaderIncluded(this IEnumerable<string> lines)
        {
            bool shouldSkip = true;
            foreach (var line in lines)
            {
                if (!shouldSkip)
                {
                    yield return line;
                }

                if (line.StartsWith("\"Data transakcji\""))
                {
                    shouldSkip = false;
                }
            }
        }

        public static IEnumerable<string> RemoveEmptyLines(this IEnumerable<string> lines) =>
            lines.Where(l => l != string.Empty);

        public static IEnumerable<string> SplitCsv(this string line) => line.Split(";");

        public static IEnumerable<string> RemoveWhitespacesAndQuotes(this IEnumerable<string> column) => 
            column
                .Select(t => t.Trim('"'))
                .Select(i => i.Trim());

        public static IEnumerable<ImportedBankTransaction> OnlyExistsingTransactions(this IEnumerable<ImportedBankTransaction> transactions) => 
            transactions.Where(t => t != null);

        public static IEnumerable<ImportedBankTransaction> AssignOrder(this IEnumerable<ImportedBankTransaction> transactions, int lastOrder)
        {
            var transactionsCreated = transactions.ToList();
            foreach (var transaction in transactionsCreated)
            {
                transaction.Order = ++lastOrder;
            }

            return transactionsCreated;
        }
    }
}
