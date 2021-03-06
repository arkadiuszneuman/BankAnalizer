﻿using BankAnalizer.Db;
using BankAnalizer.Db.Models;
using BankAnalizer.Logic.Transactions.Import.Models;
using BankAnalizer.Logic.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace BankAnalizer.Logic.Transactions.Import.Db
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

        public async Task<BankTransaction> AddToDatabase(ImportedBankTransaction transaction, Guid userId)
        {
            BankTransaction databaseTransaction = null;
            var groupDbModel = MapGroup(transaction.TransactionType);
            var bankDbModel = new Bank
            {
                Id = Guid.NewGuid(),
                Name = transaction.BankName
            };

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using var context = contextFactory.GetContext();
                var user = await userService.GetById(userId);
                context.Attach(user);

                var existingGroup = await context.BankTransactionTypes.SingleOrDefaultAsync(t => t.Name == groupDbModel.Name && t.User.Id == userId);
                var group = existingGroup ?? groupDbModel;
                group.User = user;

                var existingBank = await context.Banks.SingleOrDefaultAsync(b => b.Name == transaction.BankName);
                var bank = existingBank ?? bankDbModel;

                databaseTransaction = MapTransaction(transaction);
                databaseTransaction.BankTransactionType = group;
                databaseTransaction.User = user;
                databaseTransaction.Bank = bank;

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
                t.User.Id == userId &&
                t.BankTransactionType.Id == groupTransaction.BankTransactionType.Id &&
                t.Bank.Id == groupTransaction.Bank.Id)
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

        private BankTransaction MapTransaction(ImportedBankTransaction transaction)
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
