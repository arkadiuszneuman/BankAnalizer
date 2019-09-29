using System;
using System.Collections.Generic;
using System.Text;

namespace PkoAnalizer.Logic.Import.Models
{
    public class PkoTransaction
    {
        public DateTime OperationDate { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
    }
}
