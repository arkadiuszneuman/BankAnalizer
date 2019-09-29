using PkoAnalizer.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace PkoAnalizer.Logic.Import.Importers
{
    public class ImportException : PkoAnalizerException
    {
        public ImportException() : base() { }
        public ImportException(string message) : base(message) { }
    }
}
