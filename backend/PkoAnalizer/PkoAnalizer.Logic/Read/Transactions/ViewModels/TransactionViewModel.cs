using System;
using System.Collections.Generic;

namespace PkoAnalizer.Logic.Read.Transactions.ViewModels
{
    public class TransactionViewModel
    {
        public class TransactionGroupViewModel
        {
            public string GroupName { get; set; }
            public bool ManualGroup { get; set; }

            public TransactionGroupViewModel(string groupName, bool isManualGroup)
            {
                GroupName = groupName;
                ManualGroup = isManualGroup;
            }
        }

        public Guid TransactionId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Extensions { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public IReadOnlyCollection<TransactionGroupViewModel> Groups { get; set; } = new List<TransactionGroupViewModel>();
    }
}
