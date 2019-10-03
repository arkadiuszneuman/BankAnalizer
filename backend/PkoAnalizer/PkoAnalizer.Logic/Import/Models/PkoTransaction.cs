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
        public string Extensions { get; set; }

        //public string Location { get; set; }
        //public string PhoneNumber { get; set; }
        //public string RecipientReceipt { get; set; }
        //public string RecipientName { get; set; }
        //public string RecipientAddress { get; set; }
        //public string SenderReceipt { get; set; }
        //public string SenderName { get; set; }
        //public string SenderAddress { get; set; }
    }
}
