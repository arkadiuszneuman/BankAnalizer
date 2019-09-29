using PkoAnalizer.Logic.Import.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PkoAnalizer.Logic.Import.Importers.TypeImporters
{
    public interface ITypeImporter
    {
        public PkoTransaction Import(string[] splittedLine);
    }
}
