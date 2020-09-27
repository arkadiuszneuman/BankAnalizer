using BankAnalizer.Db;
using BankAnalizer.Db.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Z.EntityFramework.Plus;

namespace BankAnalizer.Logic.Groups.Db
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
            await using var context = contextFactory.GetContext();
            return await context.BankTransactions.SingleOrDefaultAsync(b => b.Id == bankTransactionId);
        }

        public async Task AddBankTransactionGroup(BankTransactionGroup bankTransactionGroup)
        {
            await using var context = contextFactory.GetContext();
            context.BankTransactionGroups.Add(bankTransactionGroup);
            await context.SaveChangesAsync();
        }

        public async Task<Group> GetGroupByNameAndRuleId(string groupName, Guid ruleId, Guid userId)
        {
            await using var context = contextFactory.GetContext();
            return await context.Groups
                .Include(c => c.User)
                .SingleOrDefaultAsync(b => b.Name == groupName && b.Rule.Id == ruleId && b.User.Id == userId);
        }

        public async Task<User> GetUser(Guid userId)
        {
            await using var context = contextFactory.GetContext();
            return await context.Users.FindAsync(userId);
        }

        public async Task<Group> GetGroupByName(string groupName, Guid userId)
        {
            await using var context = contextFactory.GetContext();
            return await context.Groups
                .Include(u => u.User)
                .SingleOrDefaultAsync(b => b.Name == groupName && b.User.Id == userId && b.RuleId == null);
        }

        public async Task AddBankTransactionToGroup(BankTransaction bankTransaction, Group group)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            await using var context = contextFactory.GetContext();
            context.AttachRange(bankTransaction, @group, @group.User);
            await context.AddAsync(new BankTransactionGroup
            {
                BankTransaction = bankTransaction,
                Group = @group
            });

            await context.SaveChangesAsync();
            scope.Complete();
        }

        public async Task RemoveBankTransactionsFromGroup(Guid bankTransactionId, Guid groupId)
        {
            await using var context = contextFactory.GetContext();
            await context.BankTransactionGroups
                .Where(b => b.BankTransactionId == bankTransactionId && b.GroupId == groupId)
                .DeleteAsync();
        }

        public async Task RemoveGroupIfBankTransactionsDoNotExist(Guid groupId)
        {
            await using var context = contextFactory.GetContext();
            var anyBankTransactionExists = await context.BankTransactionGroups.AnyAsync(b => b.GroupId == groupId);
            if (!anyBankTransactionExists)
                await context.Groups.Where(g => g.Id == groupId).DeleteAsync();
        }
    }
}
