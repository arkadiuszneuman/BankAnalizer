using System;

namespace PkoAnalizer.Logic.Read.Transactions.ViewModels
{
    public class TransactionsFilter
    {
        public bool OnlyWithoutGroup { get; set; }
        public string GroupName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
