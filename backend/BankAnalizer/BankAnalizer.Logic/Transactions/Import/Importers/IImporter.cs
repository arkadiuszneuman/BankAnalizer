using BankAnalizer.Logic.Transactions.Import.Models;
using System.Collections.Generic;

namespace BankAnalizer.Logic.Transactions.Import.Importers
{
    public interface IImporter
    {
        IEnumerable<PkoTransaction> ImportTransactions(string textToImport, int lastOrder);
    }
}
