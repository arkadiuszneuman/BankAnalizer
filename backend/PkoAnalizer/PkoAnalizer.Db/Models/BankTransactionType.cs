using System;
using System.Collections.Generic;
using System.Text;

namespace PkoAnalizer.Db.Models
{
    public class BankTransactionType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<BankTransaction> BankTransactions { get; set; }
    }
}
