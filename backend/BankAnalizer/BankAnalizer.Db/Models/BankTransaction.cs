using System;
using System.Collections.Generic;
using System.Text;

namespace BankAnalizer.Db.Models
{
    public class BankTransaction
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public int Order { get; set; }
        public DateTime OperationDate { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Title { get; set; }
        public string Extensions { get; set; }

        public BankTransactionType BankTransactionType { get; set; }
        public ICollection<BankTransactionGroup> BankTransactionGroups { get; set; }
    }
}
