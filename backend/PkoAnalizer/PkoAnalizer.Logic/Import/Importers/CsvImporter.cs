using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using PkoAnalizer.Logic.Common;
using PkoAnalizer.Logic.Import.Importers.TypeImporters;
using PkoAnalizer.Logic.Import.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PkoAnalizer.Logic.Import.Importers
{
    public class CsvImporter : IImporter
    {
        private readonly ILogger<CsvImporter> logger;
        private readonly IFileReader fileReader;
        private readonly IEnumerable<ITypeImporter> typeImporters;

        public CsvImporter(ILogger<CsvImporter> logger,
            IFileReader fileReader,
            IEnumerable<ITypeImporter> typeImporters)
        {
            this.logger = logger;
            this.fileReader = fileReader;
            this.typeImporters = typeImporters;
        }

        public IEnumerable<PkoTransaction> ImportTransactions(string textToImport, int lastOrder)
        {
            logger.LogInformation("Loading transactions from csv file");

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

        private PkoTransaction Import(string[] splittedLine)
        {
            PkoTransaction pkoTransaction = null;

            if (splittedLine.Any())
            {
                foreach (var typeImporter in typeImporters)
                {
                    var transaction = typeImporter.Import(splittedLine);
                    if (transaction != null)
                    {
                        if (pkoTransaction != null)
                            throw new ImportException($"Too many importers for type {splittedLine.Index(2)}");

                        pkoTransaction = transaction;
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

        public static IEnumerable<PkoTransaction> OnlyExistsingTransactions(this IEnumerable<PkoTransaction> transactions)
        {
            return transactions.Where(t => t != null);
        }

        public static IEnumerable<string> GetAllLinesExceptFirst(this IEnumerable<string> lines)
        {
            return lines.Skip(1);
        }

        public static IEnumerable<PkoTransaction> AssignOrder(this IEnumerable<PkoTransaction> transactions, int lastOrder)
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
