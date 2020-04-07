using BankAnalizer.Core;

namespace BankAnalizer.Logic.Transactions.Import.Importers
{
    public class ImportException : BankAnalizerException
    {
        public ImportException() : base() { }
        public ImportException(string message) : base(message) { }
    }
}
