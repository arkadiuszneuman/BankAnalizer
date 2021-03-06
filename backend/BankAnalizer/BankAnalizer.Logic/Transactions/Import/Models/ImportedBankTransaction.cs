﻿using System;

namespace BankAnalizer.Logic.Transactions.Import.Models
{
    public class ImportedBankTransaction
    {
        public DateTime OperationDate { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Title { get; set; }
        public string Extensions { get; set; }
        public int Order { get; set; }
        public string BankName { get; set; }
    }
}
