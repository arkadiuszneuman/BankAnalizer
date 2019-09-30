using System;
using System.Collections.Generic;
using System.Text;
using PkoAnalizer.Logic.Import.Models;

namespace PkoAnalizer.Logic.Import.Importers.TypeImporters
{
    public class StandingOrderImporter : ITypeImporter
    {
        public PkoTransaction Import(string[] splittedLine)
        {
            var type = splittedLine.Index(2);
            if (type == "Zlecenie stałe")
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
                    RecipientAddress = splittedLine.Index(8),
                    Title = splittedLine.Index(9)
                };
            }

            return null;
        }
    }
}
