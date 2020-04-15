using BankAnalizer.Logic.Transactions.Import.Models;

namespace BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters
{
    public interface ITypeImporter
    {
        public PkoTransaction Import(string[] splittedLine);
    }
}
