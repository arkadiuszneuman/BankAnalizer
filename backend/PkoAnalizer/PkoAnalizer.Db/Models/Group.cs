using System;
using System.Collections.Generic;

namespace PkoAnalizer.Db.Models
{
    public class Group
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public string Name { get; set; }
        public Rule Rule { get; set; }
        public ICollection<BankTransactionGroup> BankTransactionGroups { get; set; }
    }
}
