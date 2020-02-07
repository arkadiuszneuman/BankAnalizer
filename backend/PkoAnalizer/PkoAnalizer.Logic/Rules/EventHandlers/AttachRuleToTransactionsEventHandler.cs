using AutoMapper;
using Microsoft.Extensions.Logging;
using PkoAnalizer.Core.Commands.Group;
using PkoAnalizer.Core.Cqrs.Command;
using PkoAnalizer.Core.Cqrs.Event;
using PkoAnalizer.Logic.Import.Models;
using PkoAnalizer.Logic.Rules.Db;
using PkoAnalizer.Logic.Rules.Events;
using PkoAnalizer.Logic.Rules.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PkoAnalizer.Logic.Rules.EventHandlers
{
    public class AttachRuleToTransactionsEventHandler : IHandleEvent<RuleSavedEvent>
    {
        private readonly ILogger<AttachRuleToTransactionsEventHandler> logger;
        private readonly IMapper mapper;
        private readonly RuleParser ruleParser;
        private readonly RuleAccess ruleAccess;
        private readonly RuleMatchChecker ruleMatchChecker;
        private readonly ICommandsBus bus;

        public AttachRuleToTransactionsEventHandler(
            ILogger<AttachRuleToTransactionsEventHandler> logger,
            IMapper mapper,
            RuleParser ruleParser,
            RuleAccess ruleAccess,
            RuleMatchChecker ruleMatchChecker,
            ICommandsBus bus)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.ruleParser = ruleParser;
            this.ruleAccess = ruleAccess;
            this.ruleMatchChecker = ruleMatchChecker;
            this.bus = bus;
        }

        public async Task Handle(RuleSavedEvent @event)
        {
            var rule = mapper.Map<RuleViewModel>(@event.Rule);
            var parsedRule = ruleParser.Parse(rule);
            var bankTransactions = await ruleAccess.GetBankTransactions();

            foreach (var bankTransaction in bankTransactions)
            {
                if (ruleMatchChecker.IsRuleMatch(parsedRule, bankTransaction))
                {
                    logger.LogInformation("Found transaction {transaction} for rule {rule}", bankTransaction.Id, rule.RuleName);

                    _ = bus.Send(new AddGroupCommand(bankTransaction.Id, rule.GroupName));
                }
            }
        }
    }
}
