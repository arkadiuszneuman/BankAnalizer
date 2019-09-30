using System;
using System.Collections.Generic;
using System.Text;
using PkoAnalizer.Logic.Import.Models;

namespace PkoAnalizer.Logic.Import.Importers.TypeImporters
{
    public class TransferToAccountImporter : ITypeImporter
    {
        public PkoTransaction Import(string[] splittedLine)
        {
            var type = splittedLine.Index(2);
            if (type == "Przelew na rachunek")
            {
                return new PkoTransaction
                {
                    OperationDate = splittedLine.Index(0).ConvertToDate(),
                    TransactionDate = splittedLine.Index(1).ConvertToDate(),
                    TransactionType = splittedLine.Index(2),
                    Amount = splittedLine.Index(3).ConvertToDecimal(),
                    Currency = splittedLine.Index(4),
                    SenderReceipt = splittedLine.Index(6),
                    SenderName = splittedLine.Index(7),
                    SenderAddress = splittedLine.Index(8),
                    Title = splittedLine.Index(9)
                };
            }

            return null;
        }
    }
}
