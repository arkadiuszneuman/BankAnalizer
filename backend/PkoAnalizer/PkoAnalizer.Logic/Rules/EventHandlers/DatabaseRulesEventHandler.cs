using Microsoft.Extensions.Logging;
using PkoAnalizer.Core.Commands.Group;
using PkoAnalizer.Core.Cqrs.Command;
using PkoAnalizer.Core.Cqrs.Event;
using PkoAnalizer.Logic.Import.Events;
using PkoAnalizer.Logic.Rules.Db;
using System.Threading.Tasks;

namespace PkoAnalizer.Logic.Rules.EventHandlers
{
    public class DatabaseRulesEventHandler : IHandleEvent<TransactionSavedEvent>
    {
        private readonly ILogger<DatabaseRulesEventHandler> logger;
        private readonly RuleAccess ruleAccess;
        private readonly RuleParser ruleParser;
        private readonly RuleMatchChecker ruleMatchChecker;
        private readonly ICommandsBus bus;

        public DatabaseRulesEventHandler(ILogger<DatabaseRulesEventHandler> logger,
            RuleAccess ruleAccess,
            RuleParser ruleParser,
            RuleMatchChecker ruleMatchChecker,
            ICommandsBus bus)
        {
            this.logger = logger;
            this.ruleAccess = ruleAccess;
            this.ruleParser = ruleParser;
            this.ruleMatchChecker = ruleMatchChecker;
            this.bus = bus;
        }

        public async Task Handle(TransactionSavedEvent @event)
        {
            var rules = await ruleAccess.GetRules();
            var parsedRules = ruleParser.Parse(rules);

            foreach (var rule in parsedRules)
            {
                if (ruleMatchChecker.IsRuleMatch(rule, @event.Transaction))
                {
                    await bus.Send(new AddGroupCommand(@event.DatabaseTransaction.Id, rule.GroupName));
                }
            }
        }
    }
}
