using PkoAnalizer.Core.Commands.Import;
using PkoAnalizer.Core.Cqrs.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using PkoAnalizer.Logic.Import.Importers;

namespace PkoAnalizer.Logic.Import
{
    public class ImportCommandHandler : ICommandHandler<ImportCommand>
    {
        private readonly ILogger<ImportCommandHandler> logger;
        private readonly IEnumerable<IImporter> importers;

        public ImportCommandHandler(ILogger<ImportCommandHandler> logger,
            IEnumerable<IImporter> importers)
        {
            this.logger = logger;
            this.importers = importers;
        }

        public void Handle(ImportCommand command)
        {
            var transactions = importers.Select(i => i.ImportTransactions().ToList()).ToList();
        }
    }
}
