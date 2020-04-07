using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PkoAnalizer.Db;
using PkoAnalizer.Db.Models;
using PkoAnalizer.Logic.Transactions.Import.Models;
using PkoAnalizer.Logic.Users;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace PkoAnalizer.Logic.Transactions.Import.Db
{
    public class BankTransactionAccess
    {
        private readonly ILogger<BankTransactionAccess> logger;
        private readonly IContextFactory contextFactory;
        private readonly IUserService userService;

        public BankTransactionAccess(ILogger<BankTransactionAccess> logger,
            IContextFactory contextFactory,
            IUserService userService)
        {
            this.logger = logger;
            this.contextFactory = contextFactory;
            this.userService = userService;
        }

        public async Task<int> GetLastTransactionOrder(Guid userId)
        {
            using var context = contextFactory.GetContext();
            return (await context.BankTransactions.Where(u => u.User.Id == userId).MaxAsync(t => (int?)t.Order)) ?? 0;
        }

        public async Task<BankTransaction> AddToDatabase(PkoTransaction transaction, Guid userId)
        {
            BankTransaction databaseTransaction = null;
            var groupDbModel = MapGroup(transaction.TransactionType);

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using var context = contextFactory.GetContext();
                var user = await userService.GetById(userId);
                context.Attach(user);

                var existingGroup = context.BankTransactionTypes.SingleOrDefault(t => t.Name == groupDbModel.Name && t.User.Id == userId);
                var group = existingGroup ?? groupDbModel;
                group.User = user;

                databaseTransaction = MapTransaction(transaction);
                databaseTransaction.BankTransactionType = group;
                databaseTransaction.User = user;

                var existingTransaction = await ExistingTransaction(context, databaseTransaction, existingGroup, userId);
                if (existingTransaction == null)
                {
                    await context.AddAsync(databaseTransaction);
                    await context.SaveChangesAsync();
                    logger.LogDebug("Added transaction {transaction} to database", transaction.Title);
                }
                else
                {
                    return existingTransaction;
                }

                scope.Complete();
            }

            return databaseTransaction;
        }

        private async Task<BankTransaction> ExistingTransaction(IContext context, BankTransaction groupTransaction,
            BankTransactionType existingGroup, Guid userId)
        {
            if (existingGroup == null)
                return null;

            return await context.BankTransactions.Where(t => t.BankTransactionType.Name == existingGroup.Name &&
                t.Amount == groupTransaction.Amount &&
                t.Currency == groupTransaction.Currency &&
                t.Extensions == groupTransaction.Extensions &&
                t.OperationDate == groupTransaction.OperationDate &&
                t.Title == groupTransaction.Title &&
                t.TransactionDate == groupTransaction.TransactionDate &&
                t.User.Id == userId)
                .SingleOrDefaultAsync();
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
