using Microsoft.EntityFrameworkCore;
using PkoAnalizer.Db;
using PkoAnalizer.Db.Models;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;

namespace PkoAnalizer.Logic.GroupLogic.Db
{
    public class GroupAccess
    {
        private readonly IContextFactory contextFactory;

        public GroupAccess(IContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public async Task<BankTransaction> GetBankTransactionById(Guid bankTransactionId)
        {
            using var context = contextFactory.GetContext();
            return await context.BankTransactions.SingleOrDefaultAsync(b => b.Id == bankTransactionId);
        }

        public async Task AddBankTransactionGroup(BankTransactionGroup bankTransactionGroup)
        {
            using var context = contextFactory.GetContext();
            context.BankTransactionGroups.Add(bankTransactionGroup);
            await context.SaveChangesAsync();
        }

        public async Task<Group> GetGroupByNameAndRuleId(string groupName, Guid ruleId)
        {
            using var context = contextFactory.GetContext();
            return await context.Groups.SingleOrDefaultAsync(b => b.Name == groupName && b.Rule.Id == ruleId);
        }

        public async Task<Group> GetGroupByName(string groupName)
        {
            using var context = contextFactory.GetContext();
            return await context.Groups.SingleOrDefaultAsync(b => b.Name == groupName);
        }

        public async Task AddBankTransactionGroup(BankTransaction bankTransaction, Group group)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using var context = contextFactory.GetContext();
                context.AttachRange(bankTransaction, group);
                await context.AddAsync(new BankTransactionGroup
                {
                    BankTransaction = bankTransaction,
                    Group = group
                });

                await context.SaveChangesAsync();
                scope.Complete();
            }
        }
    }
}
