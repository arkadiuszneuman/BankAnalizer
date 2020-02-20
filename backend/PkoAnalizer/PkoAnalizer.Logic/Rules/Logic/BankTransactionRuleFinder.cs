using PkoAnalizer.Db.Models;
using PkoAnalizer.Logic.Rules.Db;
using PkoAnalizer.Logic.Rules.ViewModels;
using System.Collections.Generic;

namespace PkoAnalizer.Logic.Rules.Logic
{
    public class BankTransactionRuleFinder
    {
        private readonly RuleAccess ruleAccess;
        private readonly RuleParser ruleParser;
        private readonly RuleMatchChecker ruleMatchChecker;

        public BankTransactionRuleFinder(RuleAccess ruleAccess, RuleParser ruleParser, RuleMatchChecker ruleMatchChecker)
        {
            this.ruleAccess = ruleAccess;
            this.ruleParser = ruleParser;
            this.ruleMatchChecker = ruleMatchChecker;
        }

        public async IAsyncEnumerable<BankTransaction> FindBankTransactionsFitToRule(RuleViewModel rule)
        {
            var parsedRule = ruleParser.Parse(rule);
            var bankTransactions = await ruleAccess.GetBankTransactions();

            foreach (var bankTransaction in bankTransactions)
            {
                if (ruleMatchChecker.IsRuleMatch(parsedRule, bankTransaction))
                {
                    yield return bankTransaction;
                }
            }
        }
    }
}
