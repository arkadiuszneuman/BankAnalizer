using Microsoft.Extensions.Logging;
using PkoAnalizer.Logic.Rules.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace PkoAnalizer.Logic.Rules
{
    public enum RuleType
    {
        Contains
    }

    public class ParsedRule
    {
        public Guid Id { get; set; }
        public Guid? BankTransactionTypeId { get; set; }
        public string Column { get; set; }
        public RuleType RuleType { get; set; }
        public string Value { get; set; }
        public bool IsColumnInExtensions { get; set; }
        public string GroupName { get; set; }
        public string RuleName { get; set; }
    }

    public class RuleParser
    {
        private readonly ILogger<RuleParser> logger;

        public RuleParser(ILogger<RuleParser> logger)
        {
            this.logger = logger;
        }

        public IEnumerable<ParsedRule> Parse(IEnumerable<RuleViewModel> rules)
        {
            foreach (var rule in rules)
            {
                var parsedRule = Parse(rule);
                if (parsedRule != null)
                    yield return parsedRule;
            }
        }

        public ParsedRule Parse(RuleViewModel rule)
        {
            var splittedString = rule.RuleDefinition.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (splittedString.Length < 3)
            {
                logger.LogWarning($"Invalid rule {rule.RuleDefinition}");
                return null;
            }

            var parsedRule = new ParsedRule
            {
                Id = rule.Id,
                BankTransactionTypeId = rule.BankTransactionTypeId,
                Column = splittedString[0].Replace("Extensions.", "").Trim(),
                IsColumnInExtensions = splittedString[0].StartsWith("Extensions."),
                RuleType = Enum.Parse<RuleType>(splittedString[1].Trim()),
                Value = string.Join(" ", splittedString.Skip(2)),
                GroupName = rule.GroupName,
                RuleName = rule.RuleName
            };

            return parsedRule;
        }
    }
}
