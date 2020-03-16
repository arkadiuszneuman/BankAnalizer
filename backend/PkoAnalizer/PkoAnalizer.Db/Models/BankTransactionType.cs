using System;
using System.Collections;
using System.Collections.Generic;

namespace PkoAnalizer.Db.Models
{
    public class BankTransactionType
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public string Name { get; set; }
        public ICollection<BankTransaction> BankTransactions { get; set; }
    }
}
