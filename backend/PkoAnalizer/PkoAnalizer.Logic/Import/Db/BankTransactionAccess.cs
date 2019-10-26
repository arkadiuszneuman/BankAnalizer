using Microsoft.Extensions.Logging;
using PkoAnalizer.Db;
using PkoAnalizer.Db.Models;
using PkoAnalizer.Logic.Import.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Transactions;

namespace PkoAnalizer.Logic.Import.Db
{
    public class BankTransactionAccess
    {
        private readonly ILogger<BankTransactionAccess> logger;

        public BankTransactionAccess(ILogger<BankTransactionAccess> logger)
        {
            this.logger = logger;
        }

        public int GetLastTransactionOrder()
        {
            using var context = new PkoContext();
            return context.BankTransactions.Max(x => (int?)x.Order).GetValueOrDefault();
        }

        public async Task AddToDatabase(List<PkoTransaction> transactions)
        {
            logger.LogInformation("Starting add transactions to database");
            var groupsDbModels = MapGroups(transactions.Select(t => t.TransactionType).Distinct()).ToList();

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                foreach (var groupDbModel in groupsDbModels)
                {
                    using (var context = new PkoContext())
                    {
                        var existingGroup = context.BankTransactionTypes.SingleOrDefault(t => t.Name == groupDbModel.Name);
                        var group = existingGroup ?? groupDbModel;

                        foreach (var transaction in transactions.Where(x => x.TransactionType == group.Name))
                        {
                            var groupTransaction = MapTransaction(transaction);
                            groupTransaction.BankTransactionType = group;
                            if (!ContainsTransaction(context, groupTransaction, existingGroup))
                            {
                                await context.AddAsync(groupTransaction);
                                logger.LogDebug("Add transactions {transaction} to database", groupTransaction.Title);
                            }
                        }

                        await context.SaveChangesAsync();
                    }
                }

                scope.Complete();
            }

            logger.LogInformation("Finish adding transactions to database");
        }

        private bool ContainsTransaction(PkoContext context, BankTransaction groupTransaction, BankTransactionType existingGroup)
        {
            return existingGroup != null && context.BankTransactions.Where(t => t.BankTransactionType.Name == existingGroup.Name &&
                t.Amount == groupTransaction.Amount &&
                t.Currency == groupTransaction.Currency &&
                t.Extensions == groupTransaction.Extensions &&
                t.OperationDate == groupTransaction.OperationDate &&
                t.Title == groupTransaction.Title &&
                t.TransactionDate == groupTransaction.TransactionDate)
                .Any();
        }

        private IEnumerable<BankTransactionType> MapGroups(IEnumerable<string> groups)
        {
            foreach (var groupName in groups)
            {
                yield return new BankTransactionType
                {
                    Name = groupName
                };
            }
        }

        private BankTransaction MapTransaction(PkoTransaction transaction)
        {
            return new BankTransaction
            {
                Amount = transaction.Amount,
                Currency = transaction.Currency,
                Extensions = transaction.Extensions,
                OperationDate = transaction.OperationDate,
                Title = transaction.Title,
                TransactionDate = transaction.TransactionDate,
                Order = transaction.Order
            };
        }
    }
}
