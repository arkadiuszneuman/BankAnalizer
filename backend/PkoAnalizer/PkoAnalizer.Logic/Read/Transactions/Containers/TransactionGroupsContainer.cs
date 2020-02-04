using System;
using System.Collections.Generic;
using System.Text;

namespace PkoAnalizer.Logic.Read.Transactions.Containers
{
    public class TransactionGroupsContainer
    {
        public Guid TransactionId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string GroupName { get; set; }
    }
}
