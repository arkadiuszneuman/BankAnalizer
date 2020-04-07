using Dapper.Contrib.Extensions;
using System;

namespace BankAnalizer.Logic.Rules.ViewModels
{
    [Table("Rules")]
    public class RuleViewModel
    {
        public Guid Id { get; set; }
        public Guid? BankTransactionTypeId { get; set; }
        public string RuleDefinition { get; set; }
        public string GroupName { get; set; }
        public string RuleName { get; set; }
    }
}
