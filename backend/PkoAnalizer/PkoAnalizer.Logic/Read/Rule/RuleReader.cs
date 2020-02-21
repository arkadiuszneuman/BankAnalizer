using AutoMapper;
using PkoAnalizer.Core.ViewModels.Rules;
using PkoAnalizer.Logic.Rules;
using PkoAnalizer.Logic.Rules.Db;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PkoAnalizer.Logic.Read.Rule
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

        public async Task<IEnumerable<RuleParsedViewModel>> ReadRules()
        {
            var rules = await ruleAccess.GetRules();
            return mapper.Map<IEnumerable<RuleParsedViewModel>>(ruleParser.Parse(rules));
        }
    }
}
