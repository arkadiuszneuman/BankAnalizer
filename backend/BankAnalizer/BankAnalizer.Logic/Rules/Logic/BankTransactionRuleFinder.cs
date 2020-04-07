using AutoMapper;
using BankAnalizer.Db.Models;
using BankAnalizer.Logic.Rules.Db;
using BankAnalizer.Logic.Rules.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAnalizer.Logic.Rules.Logic
{
    public class BankTransactionRuleFinder
    {
        private readonly RuleAccess ruleAccess;
        private readonly RuleParser ruleParser;
        private readonly RuleMatchChecker ruleMatchChecker;
        private readonly IMapper mapper;

        public BankTransactionRuleFinder(RuleAccess ruleAccess,
            RuleParser ruleParser,
            RuleMatchChecker ruleMatchChecker,
            IMapper mapper)
        {
            this.ruleAccess = ruleAccess;
            this.ruleParser = ruleParser;
            this.ruleMatchChecker = ruleMatchChecker;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<BankTransaction>> FindBankTransactionsFitToRule(RuleViewModel rule, Guid userId)
        {
            var parsedRule = ruleParser.Parse(rule);
            return await FindBankTransactionsFitToRule(parsedRule, userId);
        }

        public async Task<IEnumerable<BankTransaction>> FindBankTransactionsFitToRule(ParsedRule parsedRule, Guid userId)
        {
            var bankTransactions = await ruleAccess.GetBankTransactions(userId);
            return bankTransactions.Where(bankTransaction => ruleMatchChecker.IsRuleMatch(parsedRule, bankTransaction));
        }

        public async Task<IEnumerable<BankTransaction>> FindBankTransactionsFitToRule(RuleParsedViewModel rule, Guid userId)
        {
            var parsedRule = mapper.Map<ParsedRule>(rule);
            return await FindBankTransactionsFitToRule(parsedRule, userId);
        }
    }
}
