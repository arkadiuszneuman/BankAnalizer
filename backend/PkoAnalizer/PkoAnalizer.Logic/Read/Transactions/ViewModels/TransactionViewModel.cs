using System;

namespace PkoAnalizer.Logic.Read.Transactions.ViewModels
{
    public class TransactionViewModel
    {
        public Guid TransactionId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
