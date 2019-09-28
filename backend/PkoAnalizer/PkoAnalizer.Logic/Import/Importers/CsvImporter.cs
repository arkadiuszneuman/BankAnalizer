using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using PkoAnalizer.Logic.Import.Models;

namespace PkoAnalizer.Logic.Import.Importers
{
    public class CsvImporter : IImporter
    {
        private readonly ILogger<CsvImporter> logger;

        public CsvImporter(ILogger<CsvImporter> logger)
        {
            this.logger = logger;
        }

        public IEnumerable<PkoTransaction> ImportTransactions()
        {
            logger.LogInformation("Loading transactions from csv file");
            return Enumerable.Empty<PkoTransaction>();
        }
    }
}
