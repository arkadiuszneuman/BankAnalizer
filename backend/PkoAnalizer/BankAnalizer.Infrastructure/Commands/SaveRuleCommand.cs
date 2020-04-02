using BankAnalizer.Core.Api;
using System;

namespace BankAnalizer.Infrastructure.Commands
{
    public class SaveRuleCommand : Command
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
