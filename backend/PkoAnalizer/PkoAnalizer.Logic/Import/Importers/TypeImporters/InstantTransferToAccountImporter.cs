using PkoAnalizer.Logic.Import.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PkoAnalizer.Logic.Import.Importers.TypeImporters
{
    public class InstantTransferToAccountImporter : ITypeImporter
    {
        public PkoTransaction Import(string[] splittedLine)
        {
            var type = splittedLine.Index(2);
            if (type == "Przelew Natychmiastowy na rachunek")
            {
                return new PkoTransaction
                {
                    OperationDate = splittedLine.Index(0).ConvertToDate(),
                    TransactionDate = splittedLine.Index(1).ConvertToDate(),
                    TransactionType = splittedLine.Index(2),
                    Amount = splittedLine.Index(3).ConvertToDecimal(),
                    Currency = splittedLine.Index(4),
                    RecipientReceipt = splittedLine.Index(6),
                    RecipientName = splittedLine.Index(7),
                    Title = splittedLine.Index(8)
                };
            }

            return null;
        }
    }
}
