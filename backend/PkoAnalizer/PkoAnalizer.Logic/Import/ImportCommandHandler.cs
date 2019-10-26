using PkoAnalizer.Core.Commands.Import;
using PkoAnalizer.Core.Cqrs.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using PkoAnalizer.Logic.Import.Importers;
using PkoAnalizer.Logic.Import.Db;
using System.Threading.Tasks;
using PkoAnalizer.Core.Cqrs.Event;

namespace PkoAnalizer.Logic.Import
{
    public class ImportCommandHandler : ICommandHandler<ImportCommand>
    {
        private readonly ILogger<ImportCommandHandler> logger;
        private readonly IEnumerable<IImporter> importers;
        private readonly BankTransactionAccess bankTransactionAccess;
        private readonly IEventsBus eventsBus;

        public ImportCommandHandler(ILogger<ImportCommandHandler> logger,
            IEnumerable<IImporter> importers,
            BankTransactionAccess bankTransactionAccess,
            IEventsBus eventsBus)
        {
            this.logger = logger;
            this.importers = importers;
            this.bankTransactionAccess = bankTransactionAccess;
            this.eventsBus = eventsBus;
        }

        public async Task Handle(ImportCommand command)
        {
            var lastOrder = await bankTransactionAccess.GetLastTransactionOrder();
            var transactions = importers.SelectMany(i => i.ImportTransactions(lastOrder).ToList()).ToList();
            await bankTransactionAccess.AddToDatabase(transactions);
        }
    }
}
