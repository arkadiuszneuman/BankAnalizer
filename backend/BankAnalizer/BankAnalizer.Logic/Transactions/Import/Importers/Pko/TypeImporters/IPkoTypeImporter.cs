using BankAnalizer.Logic.Transactions.Import.Models;

namespace BankAnalizer.Logic.Transactions.Import.Importers.Pko.TypeImporters
{
    public interface IPkoTypeImporter
    {
        public ImportedBankTransaction Import(string[] splittedLine);
    }
}
