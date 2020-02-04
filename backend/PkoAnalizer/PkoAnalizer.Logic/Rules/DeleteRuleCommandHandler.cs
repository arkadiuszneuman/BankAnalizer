using Microsoft.EntityFrameworkCore;
using PkoAnalizer.Core;
using PkoAnalizer.Core.Commands.Rules;
using PkoAnalizer.Core.Cqrs.Command;
using PkoAnalizer.Db;
using System.Threading.Tasks;

namespace PkoAnalizer.Logic.Rules
{
    public class DeleteRuleCommandHandler : ICommandHandler<DeleteRuleCommand>
    {
        private readonly IContextFactory contextFactory;

        public DeleteRuleCommandHandler(IContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public async Task Handle(DeleteRuleCommand command)
        {
            using var context = contextFactory.GetContext();
            var rule = await context.Rules.SingleOrDefaultAsync(r => r.Id == command.RuleId);
            if (rule == null)
                throw new PkoAnalizerException($"Invalid rule id {command.RuleId}");

            context.Rules.Remove(rule);

            await context.SaveChangesAsync();
        }
    }
}
