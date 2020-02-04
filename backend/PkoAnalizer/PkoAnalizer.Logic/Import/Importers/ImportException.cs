using PkoAnalizer.Core;

namespace PkoAnalizer.Logic.Import.Importers
{
    public class ImportException : PkoAnalizerException
    {
        public ImportException() : base() { }
        public ImportException(string message) : base(message) { }
    }
}
