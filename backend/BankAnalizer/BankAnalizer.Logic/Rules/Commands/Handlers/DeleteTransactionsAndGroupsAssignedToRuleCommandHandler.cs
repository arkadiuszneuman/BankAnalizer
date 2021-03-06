﻿using BankAnalizer.Core.Cqrs.Command;
using BankAnalizer.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Z.EntityFramework.Plus;

namespace BankAnalizer.Logic.Rules.Commands.Handlers
{
    public class DeleteTransactionsAndGroupsAssignedToRuleCommandHandler : ICommandHandler<DeleteTransactionsAndGroupsAssignedToRuleCommand>
    {
        private readonly ILogger<DeleteTransactionsAndGroupsAssignedToRuleCommandHandler> logger;
        private readonly IContextFactory contextFactory;

        public DeleteTransactionsAndGroupsAssignedToRuleCommandHandler(
            ILogger<DeleteTransactionsAndGroupsAssignedToRuleCommandHandler> logger,
            IContextFactory contextFactory)
        {
            this.logger = logger;
            this.contextFactory = contextFactory;
        }

        public async Task Handle(DeleteTransactionsAndGroupsAssignedToRuleCommand command)
        {
            using var context = contextFactory.GetContext();

            var group = await context.Groups
                .Where(g => g.Rule.Id == command.Rule.Id)
                .SingleOrDefaultAsync();

            if (group == null)
            {
                logger.LogInformation($"Group {command.Rule.GroupName} doesn't exist, continuing");
                return;
            }

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await context.BankTransactionGroups
                    .Where(b => b.GroupId == group.Id)
                    .DeleteAsync();

                context.Groups.Remove(group);
                await context.SaveChangesAsync();

                scope.Complete();
            }
        }
    }
}
