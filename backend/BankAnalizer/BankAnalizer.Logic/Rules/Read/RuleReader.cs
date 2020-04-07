using AutoMapper;
using BankAnalizer.Logic.Rules.Db;
using BankAnalizer.Logic.Rules.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankAnalizer.Logic.Rules.Read
{
    public class RuleReader
    {
        private readonly RuleAccess ruleAccess;
        private readonly RuleParser ruleParser;
        private readonly IMapper mapper;

        public RuleReader(RuleAccess ruleAccess,
            RuleParser ruleParser,
            IMapper mapper)
        {
            this.ruleAccess = ruleAccess;
            this.ruleParser = ruleParser;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<RuleParsedViewModel>> ReadRules(Guid userId)
        {
            var rules = await ruleAccess.GetRules(userId);
            return mapper.Map<IEnumerable<RuleParsedViewModel>>(ruleParser.Parse(rules));
        }
    }
}
