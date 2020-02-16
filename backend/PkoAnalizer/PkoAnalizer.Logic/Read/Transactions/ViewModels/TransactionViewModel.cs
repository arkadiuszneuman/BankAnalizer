using System;
using System.Collections.Generic;

namespace PkoAnalizer.Logic.Read.Transactions.ViewModels
{
    public class TransactionViewModel
    {
        public Guid TransactionId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Extensions { get; set; }
        public IReadOnlyCollection<string> Groups { get; set; } = new List<string>();
    }
}
