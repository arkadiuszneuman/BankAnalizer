using PkoAnalizer.Logic.Read.Rule.ViewModels;
using PkoAnalizer.Logic.Rules;
using PkoAnalizer.Logic.Rules.Db;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace PkoAnalizer.Logic.Read.Rule
{
    public class RuleReader
    {
        private readonly RuleAccess ruleAccess;
        private readonly RuleParser ruleParser;

        public RuleReader(RuleAccess ruleAccess,
            RuleParser ruleParser)
        {
            this.ruleAccess = ruleAccess;
            this.ruleParser = ruleParser;
        }

        public async Task<IEnumerable<RuleParsedViewModel>> ReadRules()
        {
            var rules = await ruleAccess.GetRules();
            return ruleParser.Parse(rules)
                .Select(r => new RuleParsedViewModel
                {
                    Id = r.Id,
                    BankTransactionTypeId = r.BankTransactionTypeId,
                    ColumnId = r.IsColumnInExtensions ? "Extensions." + r.Column : r.Column,
                    GroupName = r.GroupName,
                    RuleName = r.RuleName,
                    Text = r.Value,
                    Type = r.RuleType.ToString()
                });
        }
    }
}
