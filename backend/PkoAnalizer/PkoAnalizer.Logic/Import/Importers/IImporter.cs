using PkoAnalizer.Logic.Import.Models;
using System.Collections.Generic;

namespace PkoAnalizer.Logic.Import.Importers
{
    public interface IImporter
    {
        IEnumerable<PkoTransaction> ImportTransactions(string textToImport, int lastOrder);
    }
}
