using System;
using System.Collections.Generic;
using System.Text;

namespace PkoAnalizer.Db.Models
{
    public class Rule
    {
        public Guid Id { get; set; }
        public BankTransactionType BankTransactionType { get; set; }
        public string RuleDefinition { get; set; }
    }
}
