using System;
using System.Collections;
using System.Collections.Generic;

namespace BankAnalizer.Logic.Transactions.Read.ViewModels
{
    public class TransactionsFilter
    {
        public bool OnlyWithoutGroup { get; set; }
        public string GroupName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public IEnumerable<Guid> Users { get; set; }
    }
}
