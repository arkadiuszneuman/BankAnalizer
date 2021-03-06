﻿using BankAnalizer.Logic.Transactions.Import.Models;
using System.Collections.Generic;

namespace BankAnalizer.Logic.Transactions.Import.Importers
{
    public interface IImporter
    {
        IEnumerable<ImportedBankTransaction> ImportTransactions(TransactionsFile transactionsFile, int lastOrder);
    }
}
