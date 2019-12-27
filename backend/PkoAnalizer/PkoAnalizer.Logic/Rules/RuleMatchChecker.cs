using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using PkoAnalizer.Logic.Import.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PkoAnalizer.Logic.Rules
{
    public class RuleMatchChecker
    {
        private readonly ILogger<RuleMatchChecker> logger;

        public RuleMatchChecker(ILogger<RuleMatchChecker> logger)
        {
            this.logger = logger;
        }

        public bool IsRuleMatch(ParsedRule rule, PkoTransaction pkoTransaction)
        {
            string columnValue = null;
            if (rule.IsColumnInExtensions)
            {
                var extensions = JToken.Parse(pkoTransaction.Extensions);
                columnValue = extensions[rule.Column].Value<string>();
            }
            else
            {
                columnValue = GetStaticColumnValue(rule.Column, pkoTransaction);
            }

            if (columnValue != null)
            {
                if (rule.RuleType == RuleType.Contains)
                {
                    if (columnValue.Contains(rule.Value))
                    {
                        logger.LogInformation("Found rule for event {0} (Order: {1})", pkoTransaction.Title, pkoTransaction.Order);
                        return true;
                    }
                }
            }

            return false;
        }

        private string GetStaticColumnValue(string columnName, PkoTransaction transaction)
        {
            return columnName switch
            {
                "Title" => transaction.Title,
                _ => null
            };
        }
    }
}
