using Microsoft.EntityFrameworkCore;
using PkoAnalizer.Core;
using PkoAnalizer.Core.Cqrs.Command;
using PkoAnalizer.Db;
using System.Threading.Tasks;
using System.Transactions;

namespace PkoAnalizer.Logic.Rules.Commands.Handlers.CommandHandlers
{
    public class DeleteRuleCommandHandler : ICommandHandler<DeleteRuleCommand>
    {
        private readonly IContextFactory contextFactory;
        private readonly ICommandsBus commandsBus;

        public DeleteRuleCommandHandler(IContextFactory contextFactory,
            ICommandsBus commandsBus)
        {
            this.contextFactory = contextFactory;
            this.commandsBus = commandsBus;
        }

        public async Task Handle(DeleteRuleCommand command)
        {
            using var context = contextFactory.GetContext();
            var rule = await context.Rules.SingleOrDefaultAsync(r => r.Id == command.RuleId && r.User.Id == command.UserId);
            if (rule == null)
                throw new BankAnalizerException($"Invalid rule id {command.RuleId}");

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await commandsBus.SendAsync(new DeleteTransactionsAndGroupsAssignedToRuleCommand(rule));
                context.Rules.Remove(rule);

                await context.SaveChangesAsync();
                scope.Complete();
            }
        }
    }
}
