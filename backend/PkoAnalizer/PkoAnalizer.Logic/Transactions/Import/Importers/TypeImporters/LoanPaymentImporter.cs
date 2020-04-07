using PkoAnalizer.Core.ExtensionMethods;
using PkoAnalizer.Logic.Transactions.Import.Models;

namespace PkoAnalizer.Logic.Transactions.Import.Importers.TypeImporters
{
    public class LoanPaymentImporter : ITypeImporter
    {
        public PkoTransaction Import(string[] splittedLine)
        {
            var type = splittedLine.Index(2);
            if (type == "Spłata kredytu")
            {
                return new PkoTransaction
                {
                    OperationDate = splittedLine.Index(0).ConvertToDate(),
                    TransactionDate = splittedLine.Index(1).ConvertToDate(),
                    TransactionType = splittedLine.Index(2),
                    Amount = splittedLine.Index(3).ConvertToDecimal(),
                    Currency = splittedLine.Index(4),
                    Title = splittedLine.Index(6).RemoveSubstring("Tytuł:").Trim()
                };
            }

            return null;
        }
    }
}
