using BankAnalizer.Core.ExtensionMethods;
using BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters.Extensions;
using BankAnalizer.Logic.Transactions.Import.Models;
using System;
using System.Linq;

namespace BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters
{
    public class TransferToAccountImporter : IPkoTypeImporter
    {
        public PkoTransaction Import(string[] splittedLine)
        {
            var supportedTypes = new[] { "Przelew na rachunek", "Przelew na telefon przychodz. wew." };
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
                    Extensions = new SenderExtension
                    {
                        SenderReceipt = splittedLine.Index(6).RemoveSubstring("Rachunek nadawcy:").Trim(),
                        SenderName = splittedLine.Index(7).RemoveSubstring("Nazwa nadawcy:").Trim(),
                        SenderAddress = splittedLine.Index(8).RemoveSubstring("Adres nadawcy:").Trim()
                    }.ToJson(),
                    Title = splittedLine.Index(9).RemoveSubstring("Tytuł:").Trim()
                };
            }

            return null;
        }
    }
}
