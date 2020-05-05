using System;
using System.Collections.Generic;

namespace BankAnalizer.Db.Models
{
    public class Group
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public Rule Rule { get; set; }
        public Guid? RuleId { get; set; }
        public ICollection<BankTransactionGroup> BankTransactionGroups { get; set; }
    }
}
