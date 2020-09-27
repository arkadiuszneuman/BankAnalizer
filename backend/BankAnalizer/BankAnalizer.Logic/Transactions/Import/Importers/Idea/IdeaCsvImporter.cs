using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankAnalizer.Logic.Transactions.Import.Importers.Idea.TypeImporters;
using BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters;
using BankAnalizer.Logic.Transactions.Import.Models;
using Microsoft.Extensions.Logging;

namespace BankAnalizer.Logic.Transactions.Import.Importers.Idea
{
    public class IdeaCsvImporter : IImporter
    {
        private readonly ILogger<IdeaCsvImporter> logger;
        private readonly IEnumerable<IIdeaTypeImporter> typeImporters;

        public IdeaCsvImporter(ILogger<IdeaCsvImporter> logger,
            IEnumerable<IIdeaTypeImporter> typeImporters)
        {
            this.logger = logger;
            this.typeImporters = typeImporters;
        }

        public IEnumerable<ImportedBankTransaction> ImportTransactions(TransactionsFile transactionsFile, int lastOrder)
        {
            var fileText = transactionsFile.ReadFromUtf8();
            
            if (!fileText.StartsWith("Zestawienie transakcji"))
                return Enumerable.Empty<ImportedBankTransaction>();

            logger.LogInformation("Importing csv by IDEA Bank importer");

            return fileText.Split('\n')
                .GetAllLinesExceptFirstTwo()
                .Reverse()
                .Select(line =>
                    line.SplitCsv()
                        .ToArray())
                .Select(Import)
                .OnlyExistsingTransactions()
                .AssignOrder(lastOrder);
        }

        private ImportedBankTransaction Import(string[] splittedLine)
        {
            ImportedBankTransaction importedBankTransaction = null;

            if (splittedLine.Any() && splittedLine.Any(x => !string.IsNullOrEmpty(x)))
            {
                foreach (var typeImporter in typeImporters)
                    try
                    {
                        var transaction = typeImporter.Import(splittedLine);
                        if (transaction != null)
                        {
                            if (importedBankTransaction != null)
                                throw new ImportException($"Too many importers for type {splittedLine.Index(2)}");

                            importedBankTransaction = transaction;
                        }
                    }
                    catch (InvalidImportRowException exception)
                    {
                        logger.LogWarning(exception.Message, exception);
                    }

                if (importedBankTransaction == null)
                    logger.LogWarning($"No importer exists for type {splittedLine.Index(2)}");
            }

            return importedBankTransaction;
        }
    }

    public static class Pipe
    {
        public static IEnumerable<string> SplitCsv(this string line)
        {
            return line.Split(";");
        }

        public static IEnumerable<ImportedBankTransaction> OnlyExistsingTransactions(
            this IEnumerable<ImportedBankTransaction> transactions)
        {
            return transactions.Where(t => t != null);
        }

        public static IEnumerable<string> GetAllLinesExceptFirstTwo(this IEnumerable<string> lines)
        {
            return lines.Skip(2);
        }

        public static IEnumerable<ImportedBankTransaction> AssignOrder(
            this IEnumerable<ImportedBankTransaction> transactions, int lastOrder)
        {
            var transactionsCreated = transactions.ToList();
            foreach (var transaction in transactionsCreated) transaction.Order = ++lastOrder;

            return transactionsCreated;
        }
    }
}