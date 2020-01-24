using System;
using System.Collections.Generic;
using System.Text;

namespace PkoAnalizer.Logic.Read.Rule.ViewModels
{
    public class RuleParsedViewModel
    {
        public Guid Id { get; set; }
        public Guid? BankTransactionTypeId { get; set; }
        public string RuleName { get; set; }
        public string ColumnId { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public string GroupName { get; set; }
    }
}
