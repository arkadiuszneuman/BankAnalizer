using System;
using System.Collections;
using System.Collections.Generic;

namespace BankAnalizer.Db.Models
{
    public class BankTransactionGroup
    {
        public Guid BankTransactionId { get; set; }
        public BankTransaction BankTransaction { get; set; }
        public Guid GroupId { get; set; }
        public Group Group { get; set; }
    }
}
