using BankAnalizer.Core;

namespace BankAnalizer.Logic.Transactions.Import.Importers
{
    public class ImportException : GenericBankAnalizerException
    {
        public ImportException(string message) : base(message) { }
    }
}
