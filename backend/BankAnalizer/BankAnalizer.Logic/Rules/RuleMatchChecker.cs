using BankAnalizer.Db.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace BankAnalizer.Logic.Rules
{
    public class RuleMatchChecker
    {
        private readonly ILogger<RuleMatchChecker> logger;

        public RuleMatchChecker(ILogger<RuleMatchChecker> logger)
        {
            this.logger = logger;
        }

        public bool IsRuleMatch(ParsedRule rule, BankTransaction bankTransaction)
        {
            string columnValue = null;
            if (rule.IsColumnInExtensions)
            {
                if (bankTransaction.Extensions != null)
                {
                    var extensions = JToken.Parse(bankTransaction.Extensions);
                    var column = extensions[rule.Column];
                    if (column != null)
                        columnValue = column.Value<string>();
                }
            }
            else
            {
                columnValue = GetStaticColumnValue(rule.Column, bankTransaction);
            }

            if (columnValue != null)
            {
                if (rule.RuleType == RuleType.Contains)
                {
                    if (columnValue.ToLower().Contains(rule.Value.ToLower()))
                    {
                        logger.LogInformation("Found rule for event {0} (Order: {1})", bankTransaction.Title, bankTransaction.Order);
                        return true;
                    }
                }
            }

            return false;
        }

        private string GetStaticColumnValue(string columnName, BankTransaction transaction)
        {
            return columnName switch
            {
                "Title" => transaction.Title,
                _ => null
            };
        }
    }
}
