using BankAnalizer.Core.ExtensionMethods;
using BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters.Extensions;
using BankAnalizer.Logic.Transactions.Import.Models;

namespace BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters
{
    public class TransferFromAccountImporter : IPkoTypeImporter
    {
        public PkoTransaction Import(string[] splittedLine)
        {
            var type = splittedLine.Index(2);
            if (type == "Przelew z rachunku")
            {
                var isAddressExists = splittedLine.Index(8).Contains("Adres odbiorcy:");
                var titleIndex = isAddressExists ? 9 : 8;

                return new PkoTransaction
                {
                    OperationDate = splittedLine.Index(0).ConvertToDate(),
                    TransactionDate = splittedLine.Index(1).ConvertToDate(),
                    TransactionType = splittedLine.Index(2),
                    Amount = splittedLine.Index(3).ConvertToDecimal(),
                    Currency = splittedLine.Index(4),
                    Extensions = new RecipientExtension
                    {
                        RecipientReceipt = splittedLine.Index(6).RemoveSubstring("Rachunek odbiorcy:").Trim(),
                        RecipientName = splittedLine.Index(7).RemoveSubstring("Nazwa odbiorcy:").Trim(),
                        RecipientAddress = isAddressExists ? splittedLine.Index(8).RemoveSubstring("Adres odbiorcy:").Trim() : null,
                    }.ToJson(),
                    Title = splittedLine.Index(titleIndex).RemoveSubstring("Tytuł:").Trim()
                };
            }

            return null;
        }
    }
}
