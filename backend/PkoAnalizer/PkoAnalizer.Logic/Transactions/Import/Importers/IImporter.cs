using PkoAnalizer.Logic.Transactions.Import.Models;
using System.Collections.Generic;

namespace PkoAnalizer.Logic.Transactions.Import.Importers
{
    public interface IImporter
    {
        IEnumerable<PkoTransaction> ImportTransactions(string textToImport, int lastOrder);
    }
}
