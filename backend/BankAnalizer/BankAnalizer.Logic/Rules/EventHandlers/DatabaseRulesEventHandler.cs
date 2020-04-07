using BankAnalizer.Core.Cqrs.Command;
using BankAnalizer.Core.Cqrs.Event;
using BankAnalizer.Logic.Groups.Commands;
using BankAnalizer.Logic.Rules.Db;
using BankAnalizer.Logic.Transactions.Import.Events;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BankAnalizer.Logic.Rules.EventHandlers
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
            var rules = await ruleAccess.GetRules(@event.UserId);
            var parsedRules = ruleParser.Parse(rules);

            foreach (var rule in parsedRules)
            {
                if (ruleMatchChecker.IsRuleMatch(rule, @event.DatabaseTransaction))
                {
                    await bus.SendAsync(new AddGroupCommand(@event.DatabaseTransaction.Id, rule.GroupName, @event.UserId, rule.Id));
                }
            }
        }
    }
}
