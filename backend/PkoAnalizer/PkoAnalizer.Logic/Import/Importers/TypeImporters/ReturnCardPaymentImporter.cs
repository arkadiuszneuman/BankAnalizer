using PkoAnalizer.Core.ExtensionMethods;
using PkoAnalizer.Logic.Import.Importers.TypeImporters.Extensions;
using PkoAnalizer.Logic.Import.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PkoAnalizer.Logic.Import.Importers.TypeImporters
{
    public class ReturnCardPaymentImporter : ITypeImporter
    {
        public PkoTransaction Import(string[] splittedLine)
        {
            var type = splittedLine.Index(2);
            if (type == "Zwrot płatności kartą")
            {
                return new PkoTransaction
                {
                    OperationDate = splittedLine.Index(0).ConvertToDate(),
                    TransactionDate = splittedLine.Index(1).ConvertToDate(),
                    TransactionType = splittedLine.Index(2),
                    Amount = splittedLine.Index(3).ConvertToDecimal(),
                    Currency = splittedLine.Index(4),
                    Title = splittedLine.Index(6),
                    Extensions = new RecipientAddresExtension {
                        RecipientAddress = splittedLine.Index(7).Replace("Lokalizacja:", "").Trim()
                    }.ToJson()
                };
            }

            return null;
        }
    }
}
