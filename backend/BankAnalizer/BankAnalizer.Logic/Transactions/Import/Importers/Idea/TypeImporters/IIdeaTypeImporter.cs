using BankAnalizer.Logic.Transactions.Import.Models;

namespace BankAnalizer.Logic.Transactions.Import.Importers.Idea.TypeImporters
{
    public interface IIdeaTypeImporter
    {
        ImportedBankTransaction Import(string[] splittedLine);
    }
}