using AutoMapper;
using Microsoft.Extensions.Logging;
using PkoAnalizer.Core.Commands.Group;
using PkoAnalizer.Core.Commands.Rules;
using PkoAnalizer.Core.Cqrs.Command;
using PkoAnalizer.Core.Cqrs.Event;
using PkoAnalizer.Logic.Rules.Events;
using PkoAnalizer.Logic.Rules.Logic;
using PkoAnalizer.Logic.Rules.ViewModels;
using System.Threading.Tasks;

namespace PkoAnalizer.Logic.Rules.EventHandlers
{
    public class AttachRuleToTransactionsEventHandler : IHandleEvent<RuleSavedEvent>
    {
        private readonly ILogger<AttachRuleToTransactionsEventHandler> logger;
        private readonly IMapper mapper;
        private readonly BankTransactionRuleFinder bankTransactionRuleFinder;
        private readonly ICommandsBus bus;

        public AttachRuleToTransactionsEventHandler(
            ILogger<AttachRuleToTransactionsEventHandler> logger,
            IMapper mapper,
            BankTransactionRuleFinder bankTransactionRuleFinder,
            ICommandsBus bus)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.bankTransactionRuleFinder = bankTransactionRuleFinder;
            this.bus = bus;
        }

        public async Task Handle(RuleSavedEvent @event)
        {
            var rule = mapper.Map<RuleViewModel>(@event.Rule);

            await bus.Send(new DeleteTransactionsAndGroupsAssignedToRuleCommand(@event.Rule));

            foreach (var bankTransaction in await bankTransactionRuleFinder.FindBankTransactionsFitToRule(rule))
            {
                logger.LogInformation("Found transaction {transaction} for rule {rule}", bankTransaction.Id, rule.RuleName);

                await bus.Send(new AddGroupCommand(bankTransaction.Id, rule.GroupName, @event.Rule.Id));
            }
        }
    }
}
