using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using PkoAnalizer.Core.Cqrs.Event;
using PkoAnalizer.Logic.Import.Db;
using PkoAnalizer.Logic.Import.Events;
using PkoAnalizer.Logic.Import.Models;
using PkoAnalizer.Logic.Rules.Db;
using PkoAnalizer.Logic.Rules.ViewModels;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PkoAnalizer.Logic.Rules.EventHandlers
{
    public class DatabaseRulesEventHandler : IHandleEvent<TransactionSavedEvent>
    {
        private readonly ILogger<DatabaseRulesEventHandler> logger;
        private readonly RuleAccess ruleAccess;
        private readonly RuleParser ruleParser;
        private readonly RuleMatchChecker ruleMatchChecker;

        public DatabaseRulesEventHandler(ILogger<DatabaseRulesEventHandler> logger,
            RuleAccess ruleAccess,
            RuleParser ruleParser,
            RuleMatchChecker ruleMatchChecker)
        {
            this.logger = logger;
            this.ruleAccess = ruleAccess;
            this.ruleParser = ruleParser;
            this.ruleMatchChecker = ruleMatchChecker;
        }

        public async Task Handle(TransactionSavedEvent @event)
        {
            var rules = await ruleAccess.GetRules();
            var parsedRules = ruleParser.Parse(rules);

            foreach (var rule in parsedRules)
            {
                if (ruleMatchChecker.IsRuleMatch(rule, @event.Transaction))
                {

                }
            }
        }

        
    }
}
