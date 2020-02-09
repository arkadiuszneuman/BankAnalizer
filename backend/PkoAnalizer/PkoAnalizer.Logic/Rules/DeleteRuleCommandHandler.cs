using Microsoft.EntityFrameworkCore;
using PkoAnalizer.Core;
using PkoAnalizer.Core.Commands.Rules;
using PkoAnalizer.Core.Cqrs.Command;
using PkoAnalizer.Db;
using System.Threading.Tasks;
using System.Transactions;

namespace PkoAnalizer.Logic.Rules
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
            var rule = await context.Rules.SingleOrDefaultAsync(r => r.Id == command.RuleId);
            if (rule == null)
                throw new PkoAnalizerException($"Invalid rule id {command.RuleId}");

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await commandsBus.Send(new DeleteTransactionsAndGroupsAssignedToRuleCommand(rule));
                context.Rules.Remove(rule);

                await context.SaveChangesAsync();
                scope.Complete();
            }
        }
    }
}
