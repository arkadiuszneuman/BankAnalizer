using BankAnalizer.Core.ExtensionMethods;
using BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters.Extensions;
using BankAnalizer.Logic.Transactions.Import.Models;

namespace BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters
{
    public class CashPaymentInAtmImporter : IPkoTypeImporter
    {
        public ImportedBankTransaction Import(string[] splittedLine)
        {
            var type = splittedLine.Index(2);
            if (type == "Wpłata gotówki we wpłatomacie")
            {
                return new ImportedBankTransaction
                {
                    OperationDate = splittedLine.Index(0).ConvertToDate(),
                    TransactionDate = splittedLine.Index(1).ConvertToDate(),
                    TransactionType = splittedLine.Index(2),
                    Amount = splittedLine.Index(3).ConvertToDecimal(),
                    Currency = splittedLine.Index(4),
                    Title = splittedLine.Index(6).RemoveSubstring("Tytuł: "),
                    Extensions = new LocationExtension
                    {
                        Location = splittedLine.Index(7).RemoveSubstring("Lokalizacja: ")
                    }.ToJson()
                };
            }

            return null;
        }
    }
}
