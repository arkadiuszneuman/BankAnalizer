using PkoAnalizer.Logic.Import.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PkoAnalizer.Logic.Import.Importers
{
    public interface IImporter
    {
        IEnumerable<PkoTransaction> ImportTransactions();
    }
}
