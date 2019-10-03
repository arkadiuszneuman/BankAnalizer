using PkoAnalizer.Core.ExtensionMethods;
using PkoAnalizer.Logic.Import.Importers.TypeImporters.Extensions;
using PkoAnalizer.Logic.Import.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PkoAnalizer.Logic.Import.Importers.TypeImporters
{
    public class DirectDebitTypeImporter : ITypeImporter
    {
        public PkoTransaction Import(string[] splittedLine)
        {
            var supportedTypes = new[] { "Polecenie Zapłaty", "Przelew podatkowy" };
            var type = splittedLine.Index(2);
            if (supportedTypes.Contains(type))
            {
                return new PkoTransaction
                {
                    OperationDate = splittedLine.Index(0).ConvertToDate(),
                    TransactionDate = splittedLine.Index(1).ConvertToDate(),
                    TransactionType = splittedLine.Index(2),
                    Amount = splittedLine.Index(3).ConvertToDecimal(),
                    Currency = splittedLine.Index(4),
                    Extensions = new RecipientExtension {
                        RecipientReceipt = splittedLine.Index(6).RemoveSubstring("Rachunek odbiorcy:").Trim(),
                        RecipientName = splittedLine.Index(7).RemoveSubstring("Nazwa odbiorcy:").Trim(),
                        RecipientAddress = splittedLine.Index(8).RemoveSubstring("Adres odbiorcy:").Trim(),
                    }.ToJson(),
                    Title = splittedLine.Index(9).RemoveSubstring("Tytuł:").Trim(),
                };
            }

            return null;
        }
    }
}
