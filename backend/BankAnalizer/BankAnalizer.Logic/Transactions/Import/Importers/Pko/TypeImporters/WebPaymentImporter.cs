using BankAnalizer.Core.ExtensionMethods;
using BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters.Extensions;
using BankAnalizer.Logic.Transactions.Import.Models;
using System;
using System.Linq;

namespace BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters
{
    public class WebPaymentImporter : IPkoTypeImporter
    {
        public ImportedBankTransaction Import(string[] splittedLine)
        {
            var supportedTypes = new[] { "Płatność web - kod mobilny", "Wypłata w bankomacie - kod mobilny",
                "Anulowanie wypłaty w bankomacie - kod mobilny", "Zakup w terminalu - kod mobilny"};
            var type = splittedLine.Index(2);
            if (supportedTypes.Contains(type))
            {
                return new ImportedBankTransaction
                {
                    OperationDate = splittedLine.Index(0).ConvertToDate(),
                    TransactionDate = splittedLine.Index(1).ConvertToDate(),
                    TransactionType = splittedLine.Index(2),
                    Amount = splittedLine.Index(3).ConvertToDecimal(),
                    Currency = splittedLine.Index(4),
                    Extensions = new PhoneNumberLocationExtension
                    {
                        PhoneNumber = splittedLine.Index(6).RemoveSubstring("Numer telefonu:").Trim(),
                        Location = splittedLine.Index(7).RemoveSubstring("Lokalizacja:").Trim(),
                    }.ToJson()
                };
            }

            return null;
        }
    }
}
