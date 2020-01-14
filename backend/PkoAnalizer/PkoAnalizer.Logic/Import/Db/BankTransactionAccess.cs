using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PkoAnalizer.Db;
using PkoAnalizer.Db.Models;
using PkoAnalizer.Logic.Import.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace PkoAnalizer.Logic.Import.Db
{
    public class BankTransactionAccess
    {
        private readonly ILogger<BankTransactionAccess> logger;
        private readonly IContextFactory contextFactory;

        public BankTransactionAccess(ILogger<BankTransactionAccess> logger,
            IContextFactory contextFactory)
        {
            this.logger = logger;
            this.contextFactory = contextFactory;
        }

        public async Task<int> GetLastTransactionOrder()
        {
            using var context = contextFactory.GetContext();
            return (await context.BankTransactions.MaxAsync(t => (int?)t.Order)) ?? 0;
        }

        public async Task AddToDatabase(List<PkoTransaction> transactions)
        {
            logger.LogInformation("Starting add transactions to database");
            var groupsDbModels = MapGroups(transactions.Select(t => t.TransactionType).Distinct()).ToList();

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                foreach (var groupDbModel in groupsDbModels)
                {
                    using (var context = contextFactory.GetContext())
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


        public async Task<BankTransaction> AddToDatabase(PkoTransaction transaction)
        {
            BankTransaction databaseTransaction = null;
            var groupDbModel = MapGroup(transaction.TransactionType);

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            using var context = contextFactory.GetContext();

            var existingGroup = context.BankTransactionTypes.SingleOrDefault(t => t.Name == groupDbModel.Name);
            var group = existingGroup ?? groupDbModel;

            databaseTransaction = MapTransaction(transaction);
            databaseTransaction.BankTransactionType = group;
            if (!ContainsTransaction(context, databaseTransaction, existingGroup))
            {
                await context.AddAsync(databaseTransaction);
                await context.SaveChangesAsync();
                logger.LogDebug("Added transaction {transaction} to database", transaction.Title);
            }


            scope.Complete();

            return databaseTransaction;
        }

        private bool ContainsTransaction(IContext context, BankTransaction groupTransaction, BankTransactionType existingGroup)
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

        private BankTransactionType MapGroup(string groupName)
        {
            return new BankTransactionType
            {
                Id = Guid.NewGuid(),
                Name = groupName
            };
        }

        private BankTransaction MapTransaction(PkoTransaction transaction)
        {
            return new BankTransaction
            {
                Id = Guid.NewGuid(),
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
