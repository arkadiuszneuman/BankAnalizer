using PkoAnalizer.Core.ExtensionMethods;
using PkoAnalizer.Logic.Transactions.Import.Importers.TypeImporters.Extensions;
using PkoAnalizer.Logic.Transactions.Import.Models;

namespace PkoAnalizer.Logic.Transactions.Import.Importers.TypeImporters
{
    public class InstantTransferToAccountImporter : ITypeImporter
    {
        public PkoTransaction Import(string[] splittedLine)
        {
            var type = splittedLine.Index(2);
            if (type == "Przelew Natychmiastowy na rachunek")
            {
                return new PkoTransaction
                {
                    OperationDate = splittedLine.Index(0).ConvertToDate(),
                    TransactionDate = splittedLine.Index(1).ConvertToDate(),
                    TransactionType = splittedLine.Index(2),
                    Amount = splittedLine.Index(3).ConvertToDecimal(),
                    Currency = splittedLine.Index(4),
                    Extensions = new RecipientReceiptNameExtension
                    {
                        RecipientReceipt = splittedLine.Index(6).RemoveSubstring("Rachunek nadawcy:").Trim(),
                        RecipientName = splittedLine.Index(7).RemoveSubstring("Nazwa nadawcy:").Trim(),
                    }.ToJson(),
                    Title = splittedLine.Index(8).RemoveSubstring("Tytuł:").Trim()
                };
            }

            return null;
        }
    }
}
