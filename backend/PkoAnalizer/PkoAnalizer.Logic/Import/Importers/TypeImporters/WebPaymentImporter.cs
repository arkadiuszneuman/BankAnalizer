using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PkoAnalizer.Core.ExtensionMethods;
using PkoAnalizer.Logic.Import.Importers.TypeImporters.Extensions;
using PkoAnalizer.Logic.Import.Models;

namespace PkoAnalizer.Logic.Import.Importers.TypeImporters
{
    public class WebPaymentImporter : ITypeImporter
    {
        public PkoTransaction Import(string[] splittedLine)
        {
            var supportedTypes = new[] { "Płatność web - kod mobilny", "Wypłata w bankomacie - kod mobilny",
                "Anulowanie wypłaty w bankomacie - kod mobilny"};
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
                    Extensions = new PhoneNumberLocationExtension {
                        PhoneNumber = splittedLine.Index(6).RemoveSubstring("Numer telefonu:").Trim(),
                        Location = splittedLine.Index(7).RemoveSubstring("Lokalizacja:").Trim(),
                    }.ToJson()
                };
            }

            return null;
        }
    }
}
