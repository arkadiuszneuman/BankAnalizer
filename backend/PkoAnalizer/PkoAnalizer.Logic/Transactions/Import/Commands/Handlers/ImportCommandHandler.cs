using Microsoft.Extensions.Logging;
using PkoAnalizer.Core.Cqrs.Command;
using PkoAnalizer.Core.Cqrs.Event;
using PkoAnalizer.Logic.Transactions.Import.Db;
using PkoAnalizer.Logic.Transactions.Import.Events;
using PkoAnalizer.Logic.Transactions.Import.Importers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PkoAnalizer.Logic.Transactions.Import.Commands.Handlers
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
            logger.LogInformation("Importing transactions from file");

            var lastOrder = await bankTransactionAccess.GetLastTransactionOrder(command.UserId);
            var transactions = importers.SelectMany(i => i.ImportTransactions(command.FileText, lastOrder).ToList()).ToList();

            var transactionSavedEventTasks = new List<Task>();

            foreach (var transaction in transactions)
            {
                transaction.Order = ++lastOrder;

                var databaseTransaction = await bankTransactionAccess.AddToDatabase(transaction, command.UserId);

                if (databaseTransaction != null)
                    transactionSavedEventTasks.Add(eventsBus.Publish(new TransactionSavedEvent(command.UserId, transaction, databaseTransaction)));
                else
                    --lastOrder;
            }

            Task.WaitAll(transactionSavedEventTasks.ToArray());

            await eventsBus.Publish(new CommandCompletedEvent(command.ConnectionId, command.Id));

            logger.LogInformation("Transactions from file imported");
        }
    }
}
