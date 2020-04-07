using PkoAnalizer.Logic.Transactions.Import.Models;

namespace PkoAnalizer.Logic.Transactions.Import.Importers.TypeImporters
{
    public interface ITypeImporter
    {
        public PkoTransaction Import(string[] splittedLine);
    }
}
