using System;

namespace PkoAnalizer.Logic.Transactions.Read.Containers
{
    public class TransactionGroupsContainer
    {
        public Guid TransactionId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string GroupName { get; set; }
        public string Extensions { get; set; }
        public decimal Amount { get; set; }
        public Guid? RuleId { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
