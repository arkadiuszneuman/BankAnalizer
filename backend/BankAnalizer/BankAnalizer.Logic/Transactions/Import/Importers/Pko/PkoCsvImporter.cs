using BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters;
using BankAnalizer.Logic.Transactions.Import.Models;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankAnalizer.Logic.Transactions.Import.Importers.Pko
{
    public class PkoCsvImporter : IImporter
    {
        private readonly ILogger<PkoCsvImporter> logger;
        private readonly IEnumerable<IPkoTypeImporter> typeImporters;

        public PkoCsvImporter(ILogger<PkoCsvImporter> logger,
            IEnumerable<IPkoTypeImporter> typeImporters)
        {
            this.logger = logger;
            this.typeImporters = typeImporters;
        }

        public IEnumerable<ImportedBankTransaction> ImportTransactions(string textToImport, int lastOrder)
        {
            if (!textToImport.StartsWith("\"Data operacji\""))
                return Enumerable.Empty<ImportedBankTransaction>();

            logger.LogInformation("Importing csv by PKO BP importer");

            return textToImport.Split('\n')
                .GetAllLinesExceptFirst()
                .Reverse()
                .Select(line =>
                    line.SplitCsv()
                        .RemoveWhitespacesAndQuotes()
                        .RemoveEmptyEntries()
                        .ToArray())
                .Select(Import)
                .OnlyExistsingTransactions()
                .AssignOrder(lastOrder);
        }

        private ImportedBankTransaction Import(string[] splittedLine)
        {
            ImportedBankTransaction pkoTransaction = null;

            if (splittedLine.Any())
            {
                foreach (var typeImporter in typeImporters)
                {
                    try
                    {
                        var transaction = typeImporter.Import(splittedLine);
                        if (transaction != null)
                        {
                            if (pkoTransaction != null)
                                throw new ImportException($"Too many importers for type {splittedLine.Index(2)}");

                            pkoTransaction = transaction;
                        }
                    }
                    catch (InvalidImportRowException exception)
                    {
                        logger.LogError(exception.Message, exception);
                    }
                }

                if (pkoTransaction == null)
                    logger.LogWarning($"No importer exists for type {splittedLine.Index(2)}");
            }

            return pkoTransaction;
        }
    }

    public static class Pipe
    {
        public static IEnumerable<string> SplitCsv(this string line)
        {
            return line.Split("\",\"");
        }

        public static IEnumerable<string> RemoveWhitespacesAndQuotes(this IEnumerable<string> column)
        {
            return column
                .Select(i => i.Trim())
                .Select(t => t.Trim('"'));
        }

        public static IEnumerable<string> RemoveEmptyEntries(this IEnumerable<string> column)
        {
            return column
                .Where(c => c != string.Empty);
        }

        public static IEnumerable<ImportedBankTransaction> OnlyExistsingTransactions(this IEnumerable<ImportedBankTransaction> transactions)
        {
            return transactions.Where(t => t != null);
        }

        public static IEnumerable<string> GetAllLinesExceptFirst(this IEnumerable<string> lines)
        {
            return lines.Skip(1);
        }

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
