using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PkoAnalizer.Logic.Rules.ViewModels
{
    [Table("Rules")]
    public class RuleViewModel
    {
        public Guid Id { get; set; }
        public Guid? BankTransactionTypeId { get; set; }
        public string RuleDefinition { get; set; }
    }
}
