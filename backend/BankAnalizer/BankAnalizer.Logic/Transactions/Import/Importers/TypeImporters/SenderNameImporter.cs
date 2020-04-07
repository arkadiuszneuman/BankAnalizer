using BankAnalizer.Core.ExtensionMethods;
using BankAnalizer.Logic.Transactions.Import.Importers.TypeImporters.Extensions;
using BankAnalizer.Logic.Transactions.Import.Models;
using System;
using System.Linq;

namespace BankAnalizer.Logic.Transactions.Import.Importers.TypeImporters
{
    public class SenderNameImporter : ITypeImporter
    {
        public PkoTransaction Import(string[] splittedLine)
        {
            var supportedTypes = new[] { "Wpłata gotówkowa w kasie" };
            var type = splittedLine.Index(2);
            if (supportedTypes.Contains(type))
            {
                return new PkoTransaction
                {
                    OperationDate = splittedLine.Index(0).ConvertToDate(),
                    TransactionDate = splittedLine.Index(1).ConvertToDate(),
                    TransactionType = splittedLine.Index(2),
                    Amount = splittedLine.Index(3).ConvertToDecimal(),
                    Currency = splittedLine.Index(4),
                    Extensions = new SenderNameExtension
                    {
                        SenderName = splittedLine.Index(6).RemoveSubstring("Nazwa nadawcy:").Trim()
                    }.ToJson(),
                    Title = splittedLine.Index(8).RemoveSubstring("Tytuł:").Trim()
                };
            }

            return null;
        }
    }
}
