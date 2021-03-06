﻿using System;

namespace BankAnalizer.Db.Models
{
    public class Rule
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public BankTransactionType BankTransactionType { get; set; }
        public string RuleDefinition { get; set; }
        public string GroupName { get; set; }
        public string RuleName { get; set; }
    }
}
