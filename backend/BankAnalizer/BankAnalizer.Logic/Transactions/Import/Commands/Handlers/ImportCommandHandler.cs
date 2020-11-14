using BankAnalizer.Core.Cqrs.Command;
using BankAnalizer.Core.Cqrs.Event;
using BankAnalizer.Logic.CommonEvents;
using BankAnalizer.Logic.Transactions.Import.Db;
using BankAnalizer.Logic.Transactions.Import.Events;
using BankAnalizer.Logic.Transactions.Import.Importers;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAnalizer.Logic.Transactions.Import.Commands.Handlers
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
            var transactions = importers.SelectMany(i => i.ImportTransactions(command.TransactionsFile, lastOrder).ToList()).ToList();

            foreach (var transaction in transactions)
            {
                transaction.Order = ++lastOrder;

                var databaseTransaction = await bankTransactionAccess.AddToDatabase(transaction, command.UserId);

                if (databaseTransaction != null)
                    await eventsBus.Publish(new TransactionSavedEvent(command.UserId, transaction, databaseTransaction));
                else
                    --lastOrder;
            }

            await eventsBus.Publish(new CommandCompletedEvent(command.UserId, command.Id));

            logger.LogInformation("Transactions from file imported");
        }
    }
}
